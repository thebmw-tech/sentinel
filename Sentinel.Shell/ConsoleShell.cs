using System;
using System.IO;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Models;
using Sentinel.Core.Command.Services;

namespace Sentinel.Shell
{
    public class ConsoleShell : IShell
    {
        private readonly CommandInterpreter interpreter;

        public ConsoleShell(CommandInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public ShellContext Context { get; set; }
        public CommandMode CommandMode { get; set; } = CommandMode.Shell;

        public TextWriter Out => Console.Out;
        public TextWriter Error => Console.Error;

        public void ShellLoop(Func<CommandMode, string> getPrompt)
        {
            
            while (true)
            {
                var prompt = getPrompt(CommandMode);
                var command = GetCommandFromConsole(CommandMode, prompt);

                var commandResult = interpreter.Execute(this, CommandMode, command);

                if (commandResult == CommandReturn.Exit)
                {
                    break;
                }
            }
        }

        private string GetCommandFromConsole(CommandMode mode, string prompt)
        {
            Console.Write(prompt);

            string command = "";

            while (true)
            {
                var key = Console.ReadKey(true);
                var pos = Console.GetCursorPosition();

                switch (key)
                {
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.Backspace:
                        if (command.Length > 0)
                        {
                            command = command.Substring(0, command.Length - 1);
                            Console.SetCursorPosition(pos.Left - 1, pos.Top);
                            Console.Write(' ');
                            Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        }
                        else
                        {
                            // Sound Bell
                            Console.Write('\a');
                        }

                        break;
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.Enter:
                        Console.WriteLine();
                        return command;
                    case ConsoleKeyInfo k when k.Key == ConsoleKey.Tab:
                        command = interpreter.Suggest(this, CommandMode, command);
                        Console.SetCursorPosition(prompt.Length, pos.Top);
                        Console.Write($"{command}");
                        break;
                    case ConsoleKeyInfo k when k.KeyChar == '?':
                        Console.WriteLine();
                        interpreter.Help(this, CommandMode, command);
                        Console.Write($"{prompt}{command}");
                        break;
                    case ConsoleKeyInfo k when k.KeyChar != 0 && (k.Modifiers == 0 || k.Modifiers == ConsoleModifiers.Shift):
                        command += k.KeyChar;
                        Console.Write(k.KeyChar);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}