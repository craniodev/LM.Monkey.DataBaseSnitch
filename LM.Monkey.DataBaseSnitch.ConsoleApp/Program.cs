using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using LM.Monkey.DataBaseSnitch.ConsoleApp.Models;

namespace LM.Monkey.DataBaseSnitch.ConsoleApp
{
    class Program
    {
        const string BREAK_ = "-------------------------------------------- - ";



        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                    .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
                     .WithNotParsed<Options>((errs) => HandleParseError(errs));




        }

        private static void HandleParseError(IEnumerable<Error> errs)
        {

            Environment.Exit(1);
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {


            const string BREAK_ = "-------------------------------------------- - ";



            DataBase source = null;
            DataBase target = null;


            if (string.Equals(opts.SourceType, "files", StringComparison.OrdinalIgnoreCase))
            {
                source = DataBaseFileReader.Read(opts.Source);
            }
            else
            {
                source = DataBaseSQLReader.Read(opts.Source);
            }

            if (string.Equals(opts.TargetType, "files", StringComparison.OrdinalIgnoreCase))
            {
                target = DataBaseFileReader.Read(opts.Target);
            }
            else
            {
                target = DataBaseSQLReader.Read(opts.Target);
            }


            var compareSource = DataBaseCompare.Compare(source, target);
            var compareTarget = DataBaseCompare.Compare(target, source);

            Display("Source", source, compareSource);
            Display("Target", target, compareTarget);

            if (compareSource.Count > 0 | compareTarget.Count > 0)
            {
                Environment.Exit(0);
            }
            else
            {
                Environment.Exit(1);
            }

        }

        private static void Display(string name, DataBase dataBase, Dictionary<string, string> compare)
        {
            Console.WriteLine(BREAK_);
            Console.WriteLine(name);
            Console.WriteLine(" Tables: {0}", dataBase.Objects.Count(x => x.Type == DataBaseObjectType.Table));
            Console.WriteLine(" Procedure: {0}", dataBase.Objects.Count(x => x.Type == DataBaseObjectType.Procedure));
            Console.WriteLine(" View: {0}", dataBase.Objects.Count(x => x.Type == DataBaseObjectType.View));
            Console.WriteLine(" Function: {0}", dataBase.Objects.Count(x => x.Type == DataBaseObjectType.Function));
            if (compare.Count == 0)
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" equals");
                Console.ResetColor();


            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Errors: {0}", compare.Count);
                Console.ResetColor();

                foreach (var obj in compare)
                {
                    Console.WriteLine("     {0}: {1}", obj.Key, obj.Value);
                }
            }
        }
    }

}
