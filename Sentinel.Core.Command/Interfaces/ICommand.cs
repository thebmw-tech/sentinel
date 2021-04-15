using System;
using System.IO;
using System.Threading.Tasks;
using Sentinel.Core.Command.Enums;

namespace Sentinel.Core.Command.Interfaces
{
    public interface ICommand
    {
        int Main(string[] args, TextReader input, TextWriter output, TextWriter error);

        string Suggest(string[] args);
    }
}