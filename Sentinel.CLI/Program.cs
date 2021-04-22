using System;
using System.Linq;
using System.Reflection;

namespace Sentinel.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Missing Module Argument");
                return;
            }
            
            var moduleName = args[0];
            var moduleArgs = args.Skip(1).ToArray();

            RunModule(moduleName, moduleArgs);
        }

        static void RunModule(string moduleName, string[] moduleArgs)
        {
            var moduleType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == moduleName);
            if (moduleType == null)
            {
                Console.Error.WriteLine($"Failed to find module {moduleName}");
                return;
            }

            var moduleInstance = (IModule) Activator.CreateInstance(moduleType);

            if (moduleInstance == null)
            {
                Console.Error.WriteLine($"Filed to load module {moduleName}");
                return;
            }

            moduleInstance.Main(moduleArgs);
        }
    }
}
