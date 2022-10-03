using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sentinel.Core.Command.Enums;
using Sentinel.Core.Command.Interfaces;
using Sentinel.Core.Command.Models;
using Sentinel.Core.Command.Services;
using Sentinel.Core.Factories;
using Sentinel.Core.Repository;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services.Interfaces;
using Sentinel.Models;

namespace Sentinel.Shell
{
    public class ConsoleShell : IShell
    {
        private readonly CommandInterpreter interpreter;
        private readonly ISystemConfigurationRepository systemConfigurationRepository;
        private readonly IRevisionService revisionService;
        private readonly EnvironmentSetupFactory environmentSetupFactory;

        private bool inLoop = true;

        public ConsoleShell(CommandInterpreter interpreter, ISystemConfigurationRepository systemConfigurationRepository, IRevisionService revisionService, EnvironmentSetupFactory environmentSetupFactory)
        {
            this.interpreter = interpreter;
            this.systemConfigurationRepository = systemConfigurationRepository;
            this.revisionService = revisionService;
            this.environmentSetupFactory = environmentSetupFactory;

            Environment = new ConcurrentDictionary<string, object>();
        }

        

        public IDictionary<string,object> Environment { get; set; }
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

        public T GetEnvironment<T>(string key)
        {
            if (!Environment.ContainsKey(key)) return default;
            return (T) Environment[key];
        }

        private string GetPrompt(CommandMode mode)
        {
            var hostname = "";
            try
            {
                var configuration = systemConfigurationRepository.GetCurrent();
                hostname = configuration.Hostname;
            }
            catch
            {
                Console.Error.WriteLine("ERROR LOADING CONFIGURATION");
                Console.Error.WriteLine("You may need to run database migrations manually");
                hostname = System.Environment.MachineName;
            }

            var prompt = environmentSetupFactory.Build(mode).GetPrompt(this, hostname);

            return $"{prompt} ";
        }

        private void ShellBackgroundTask()
        {
            while (inLoop)
            {
                if (Environment.ContainsKey("CONFIG_REVISION_ID"))
                {
                    var revisionId = (int) Environment["CONFIG_REVISION_ID"];
                    System.Diagnostics.Debug.WriteLine($"Updating revision {revisionId} lock.");
                    revisionService.UpdateRevisionLock(revisionId);
                }
                
                Thread.Sleep(30000);
            }
        }

        public void ShellLoop()
        {
            Task.Run(ShellBackgroundTask);

            while (inLoop)
            {
                var prompt = GetPrompt(CommandMode);
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
                    case {Key: ConsoleKey.LeftArrow}:
                        if (commandPos > 0)
                        {
                            Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        }
                        else
                        {
                            Bell();
                        }
                        break;
                    case {Key: ConsoleKey.RightArrow}:
                        if (commandPos < command.Count)
                        {
                            Console.SetCursorPosition(pos.Left + 1, pos.Top);
                        }
                        else
                        {
                            Bell();
                        }
                        break;
                    case {Key: ConsoleKey.Home}:
                        Console.SetCursorPosition(prompt.Length, pos.Top);
                        break;
                    case {Key: ConsoleKey.End}:
                        Console.SetCursorPosition(prompt.Length + command.Count, pos.Top);
                        break;
                    case {Key: ConsoleKey.Backspace}:
                        if (commandPos > 0)
                        {
                            command.RemoveAt(commandPos - 1);
                            OptimizedReplace(prompt.Length, commandPos - 1, commandAsString());
                            Console.Write(' ');
                            Console.SetCursorPosition(pos.Left - 1, pos.Top);
                        }
                        else
                        {
                            // Sound Bell
                            Bell();
                        }
                        break;
                    case {Key: ConsoleKey.Delete}:
                        if (command.Count > 0 && commandPos < command.Count)
                        {
                            command.RemoveAt(commandPos);
                            OptimizedReplace(prompt.Length, commandPos, commandAsString());
                            Console.Write(' ');
                            Console.SetCursorPosition(pos.Left, pos.Top);
                        }
                        else
                        {
                            // Sound Bell
                            Bell();
                        }
                        break;
                    case {Key: ConsoleKey.Enter}:
                        Console.WriteLine();
                        return commandAsString();
                    case {Key: ConsoleKey.Tab}:
                        var currentCommandString = commandAsString();
                        var tabSuggestion = interpreter.Suggest(this, CommandMode, currentCommandString);
                        command = new List<char>(tabSuggestion.ToCharArray());
                        commandPos = currentCommandString.Zip(tabSuggestion, (c1, c2) => c1 == c2).TakeWhile(b => b).Count();
                        OptimizedReplace(prompt.Length, commandPos, tabSuggestion);
                        break;
                    case {KeyChar: '?'}:
                        Console.WriteLine();
                        interpreter.Help(this, CommandMode, commandAsString());
                        Console.Write($"{prompt}{commandAsString()}");
                        break;
                    case { } k when k.KeyChar != 0 && (k.Modifiers is 0 or ConsoleModifiers.Shift):
                        command.Insert(commandPos, k.KeyChar);
                        if (command.Count == commandPos)
                        {
                            Console.Write(k.KeyChar);
                        }
                        else
                        {
                            OptimizedReplace(prompt.Length, commandPos, commandAsString());
                            Console.SetCursorPosition(pos.Left + 1, pos.Top);
                        }

                        break;
                    default:
                        break;
                }
            }
        }

        private static void OptimizedReplace(int promptLength, int commandPos, string commandString)
        {
            var (_, top) = Console.GetCursorPosition();
            Console.SetCursorPosition(promptLength + commandPos, top);
            if (commandString.Length > 0)
            {
                Console.Write(commandString[commandPos..]);
            }
        }

        private static void Bell()
        {
            Console.Write('\a');
        }
    }
}