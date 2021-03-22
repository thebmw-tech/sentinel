using System;
using System.Text.RegularExpressions;

namespace Sentinel.Shell
{
    public class Shell
    {
        private String hostname = "hostname";

        private readonly CommandInterpreter interpreter;

        public Shell(CommandInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public void ShellLoop()
        {
            while (true)
            {
                String prompt = $"{hostname}> ";

                Console.Write(prompt);

                

                string command = "";
                while (true)
                {
                    var key = Console.ReadKey();

                    if (key.KeyChar == '?')
                    {
                        Console.WriteLine();
                        interpreter.Help(CommandMode.Shell, command);
                        Console.Write($"{prompt}{command}");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                    else
                    {
                        command += key.KeyChar;
                    }
                }
                
                interpreter.Execute(CommandMode.Shell, command);
                
            }
        }
    }
}