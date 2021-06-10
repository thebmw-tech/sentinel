using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Sentinel.Core.Helpers
{
    public static class HelperFunctions
    {

        public static string FindExistingFile(string[] files, string defaultString)
        {
            foreach (var file in files)
            {
                if (System.IO.File.Exists(file))
                {
                    return file;
                }
            }

            return defaultString;
        }

        public static void MigrateDatabase(TextWriter output = null)
        {
            output ??= Console.Out;

            output.WriteLine("Checking For Available Database Migrations");

            using var ctx = new SentinelDatabaseContext();
            var migrations = ctx.Database.GetPendingMigrations();
            output.WriteLine("Found the following migrations to execute");
            foreach (var migration in migrations)
            {
                output.WriteLine($"\t{migration}");
            }

            output.Write("Executing Migrations...");
            var start = DateTime.Now;
            ctx.Database.Migrate();
            output.WriteLine($"\tDone[{DateTime.Now - start}]");
        }

        public static void VerifyFirstRun(ServiceProvider services)
        {
            var ctx = services.GetService<SentinelDatabaseContext>();
            if (!ctx.Database.GetAppliedMigrations().Any())
            {
                MigrateDatabase();
                DefaultConfigurationSeed.Seed(services);
            }
        }

        public static IEnumerable<string> ParseArguments(this string commandLine)
        {
            if (string.IsNullOrWhiteSpace(commandLine))
                yield break;

            var sb = new StringBuilder();
            bool inQuote = false;
            foreach (char c in commandLine)
            {
                if (c == '"' && !inQuote)
                {
                    inQuote = true;
                    continue;
                }

                if (c != '"' && !(char.IsWhiteSpace(c) && !inQuote))
                {
                    sb.Append(c);
                    continue;
                }

                if (sb.Length > 0)
                {
                    var result = sb.ToString();
                    sb.Clear();
                    inQuote = false;
                    yield return result;
                }
            }

            if (sb.Length > 0)
                yield return sb.ToString();
        }

        public static string LCDString(List<string> str)
        {
            if (str.Count == 0) return "";
            if (str.Count == 1) return str.First();

            for (int i = 1; i <= str.First().Length; i++)
            {
                if (!str.TrueForAll(s => s.StartsWith(str.First().Substring(0, i))))
                {
                    return str.First().Substring(0, i - 1);
                }
            }

            return "";
        }

        public static string GetSubCommand(string command)
        {
            var firstSpaceIndex = command.IndexOf(' ');
            if (firstSpaceIndex == -1)
                return "";
            return command.Substring(firstSpaceIndex + 1);
        }

        public static void SetProperty<TObject>(TObject obj, String propertyName, String propertyValue)
        {
            var objType = typeof(TObject);
            var propertyInfo = objType.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException("Could not find property.", propertyName);
            }

            object value = null;
            if (propertyValue != "null")
            {
                if (propertyInfo.PropertyType.IsConstructedGenericType)
                {
                    var propertyType = propertyInfo.PropertyType.GenericTypeArguments[0];
                    value = Convert.ChangeType(propertyValue, propertyType);
                }
                else
                {
                    value = Convert.ChangeType(propertyValue, propertyInfo.PropertyType);
                }

                if (value == null)
                {
                    throw new ArgumentException($"Invalid value {propertyValue}.", propertyName);
                }
            }

            propertyInfo.SetValue(obj, value);
        }

        public static Tuple<string, string[]> ParseCommandWithArgs(string commandWithArgs)
        {
            commandWithArgs = commandWithArgs.Trim();
            var firstSpaceIndex = commandWithArgs.IndexOf(' ');
            if (firstSpaceIndex == -1)
            {
                return new Tuple<string, string[]>(commandWithArgs, new string[] { });
            }
            var command = commandWithArgs.Substring(0, firstSpaceIndex);
            var argsString = commandWithArgs.Substring(firstSpaceIndex + 1);
            var args = argsString.ParseArguments().ToArray();
            return new Tuple<string, string[]>(command, args);
        }

        public static bool EnumIsEqual<TEnum>(TEnum enumValue, string value)
        {
            if (!Enum.TryParse(typeof(TEnum), value, out var eResult))
            {
                return false;
            }

            var enumToCheck = (TEnum) eResult;
            return enumToCheck.Equals(enumValue);
        }

    }
}