using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sentinel.Core.Helpers
{
    public class ConsoleFormatHelper
    {
        public static void WriteSpacedTuples(List<Tuple<string, string>> tuples, TextWriter output)
        {
            if (tuples.Count == 0) return;

            tuples = tuples.OrderBy(t => t.Item1).ToList();

            var firstColumnSpacing = tuples.Max(t => t.Item1.Length) + 3;

            foreach (var tuple in tuples)
            {
                var spacing = firstColumnSpacing - tuple.Item1.Length;
                output.Write(' ');
                output.Write(tuple.Item1);
                for (int i = 0; i < spacing; i++) output.Write(' ');
                output.WriteLine(tuple.Item2);
            }
        }
    }
}