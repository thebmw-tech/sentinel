using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sentinel.Core.Helpers
{
    public static class HelperFunctions
    {

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

    }
}