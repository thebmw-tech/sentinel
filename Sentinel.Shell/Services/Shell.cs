using System;
using Sentinel.Shell.Enums;
using Sentinel.Shell.Models;

namespace Sentinel.Shell.Services
{
    public class Shell
    {
        private readonly CommandInterpreter interpreter;

        public Shell(CommandInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public void ShellLoop(CommandMode mode, ShellContext context, Func<string> getPrompt)
        {
            while (true)
            {
                var prompt = getPrompt();
                var command = GetCommandFromConsole(mode, prompt);

                var commandResult = interpreter.Execute(mode, context, command);

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
                        command = interpreter.Suggest(mode, command);
                        Console.SetCursorPosition(prompt.Length, pos.Top);
                        Console.Write($"{command}");
                        break;
                    case ConsoleKeyInfo k when k.KeyChar == '?':
                        Console.WriteLine();
                        interpreter.Help(mode, command);
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