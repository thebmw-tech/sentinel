using System;
using System.Collections.Generic;
using System.Linq;

namespace Sentinel.Shell.Helpers
{
    public class ConsoleFormatHelper
    {
        public static void WriteSpacedTuples(List<Tuple<string, string>> tuples)
        {
            tuples = tuples.OrderBy(t => t.Item1).ToList();

            var firstColumnSpacing = tuples.Max(t => t.Item1.Length) + 3;

            foreach (var tuple in tuples)
            {
                var spacing = firstColumnSpacing - tuple.Item1.Length;
                Console.Write(' ');
                Console.Write(tuple.Item1);
                for (int i = 0; i < spacing; i++) Console.Write(' ');
                Console.WriteLine(tuple.Item2);
            }
        }
    }
}