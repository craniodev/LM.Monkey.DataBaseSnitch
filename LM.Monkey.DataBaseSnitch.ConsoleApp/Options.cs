using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Monkey.DataBaseSnitch.ConsoleApp
{
    class Options
    {
 
        [Option(Default = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option("sourceType", Default = "File", HelpText = "Source Type: st File / st DataBase")]
        public string SourceType { get; set; }

        [Option("source", Required = true, HelpText = "Source: Path to files / Connection string to DataBase")]
        public string Source { get; set; }

        [Option("targetType", Default = "DataBase", HelpText = "Source Type: st File / st DataBase")]
        public string TargetType { get; set; }

        [Option("target", Required = true, HelpText = "Source: Path to files / Connection string to DataBase")]
        public string Target { get; set; }
    }
}
