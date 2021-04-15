﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Models;
using Sentinel.Core.Command.Services;

namespace Sentinel.Shell
{
    public class ConsoleShell : IShell
    {
        private const int HIST_LENGTH = 1000;

        private readonly CommandInterpreter interpreter;

        private bool inLoop = true;

        public ConsoleShell(CommandInterpreter interpreter)
        {
            this.interpreter = interpreter;

            Environment = new Dictionary<string, object>();
        }

        

        public Dictionary<string,object> Environment { get; set; }
        public CommandMode CommandMode { get; set; } = CommandMode.Shell;

        public TextWriter Output => Console.Out;
        public TextWriter Error => Console.Error;

        public void SYS_SetCommandMode(CommandMode commandMode)
        {
            CommandMode = commandMode;
        }

        public void SYS_ExitShell()
        {
            inLoop = false;
        }

        public void ShellLoop(Func<CommandMode, string> getPrompt)
        {

            while (inLoop)
            {
                var prompt = getPrompt(CommandMode);
                var commandLine = GetCommandLineFromConsole(CommandMode, prompt);

                var commandResult = interpreter.Execute(this, CommandMode, commandLine);

                Environment["LAST_EXIT_CODE"] = commandResult;
            }
        }

        private string GetCommandLineFromConsole(CommandMode mode, string prompt)
        {
            Console.Write(prompt);

            List<char> command = new List<char>();

            var commandAsString = new Func<string>(() => new string(command.ToArray()));

            while (true)
            {
                var key = Console.ReadKey(true);
                var pos = Console.GetCursorPosition();
                var commandPos = pos.Left - prompt.Length;

                switch (key)
                {
                    // Handle Direction Keys
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.LeftArrow:
                        if (commandPos > 0)
                        {
                            Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        }
                        else
                        {
                            Bell();
                        }
                        break;
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.RightArrow:
                        if (commandPos < command.Count)
                        {
                            Console.SetCursorPosition(pos.Left + 1, pos.Top);
                        }
                        else
                        {
                            Bell();
                        }
                        break;
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.Home:
                        Console.SetCursorPosition(prompt.Length, pos.Top);
                        break;
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.End:
                        Console.SetCursorPosition(prompt.Length + command.Count, pos.Top);
                        break;
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.Backspace:
                        if (command.Count > 0)
                        {
                            command.RemoveAt(commandPos - 1);
                            System.Diagnostics.Debug.WriteLine(commandAsString());
                            Console.SetCursorPosition(prompt.Length, pos.Top);
                            Console.Write(commandAsString());
                            Console.Write(' ');
                            Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        }
                        else
                        {
                            // Sound Bell
                            Bell();
                        }
                        break;
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.Delete:
                        if (command.Count > 0 && commandPos < command.Count)
                        {
                            command.RemoveAt(commandPos);
                            System.Diagnostics.Debug.WriteLine(commandAsString());
                            Console.SetCursorPosition(prompt.Length, pos.Top);
                            Console.Write(commandAsString());
                            Console.Write(' ');
                            Console.SetCursorPosition(pos.Left, pos.Top);
                        }
                        else
                        {
                            // Sound Bell
                            Bell();
                        }
                        break;
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.Enter:
                        Console.WriteLine();
                        return commandAsString();
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.Tab:
                        var tabSuggestion = interpreter.Suggest(this, CommandMode, commandAsString());
                        command = new List<char>(tabSuggestion.ToCharArray());
                        Console.SetCursorPosition(prompt.Length, pos.Top);
                        Console.Write($"{commandAsString()}");
                        break;
                    case ConsoleKeyInfo k when k.KeyChar == '?':
                        Console.WriteLine();
                        interpreter.Help(this, CommandMode, commandAsString());
                        Console.Write($"{prompt}{commandAsString()}");
                        break;
                    case ConsoleKeyInfo k when k.KeyChar != 0 && (k.Modifiers == 0 || k.Modifiers == ConsoleModifiers.Shift):
                        command.Insert(commandPos, k.KeyChar);
                        if (command.Count == commandPos)
                        {
                            Console.Write(k.KeyChar);
                        }
                        else
                        {
                            Console.SetCursorPosition(prompt.Length, pos.Top);
                            Console.Write(commandAsString());
                            Console.SetCursorPosition(pos.Left + 1, pos.Top);
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        private static void Bell()
        {
            Console.Write('\a');
        }
    }
}