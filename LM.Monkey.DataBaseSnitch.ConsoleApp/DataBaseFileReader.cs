using LM.Monkey.DataBaseSnitch.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Monkey.DataBaseSnitch.ConsoleApp
{
    internal static class DataBaseFileReader
    {
        public static DataBase Read(string path)
        {



            var dataBase = new DataBase();

            if (File.Exists(@path))
            {
                // This path is a file
                ProcessFile(dataBase, @path);
            }
            else if (Directory.Exists(@path))
            {
                // This path is a directory
                ProcessDirectory(dataBase, @path);
            }
            else
            {
                // Console.WriteLine("{0} is not a valid file or directory.", path);
            }
            return dataBase;
        }

        public static void ProcessDirectory(DataBase dataBase, string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(dataBase, fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(dataBase, subdirectory);
        }

        public static void ProcessFile(DataBase dataBase, string path)
        {
            if (Path.GetExtension(path) != ".sql") return;

            var t = DataBaseObjectType.Procedure;

            if (path.IndexOf("Stored Procedures", StringComparison.OrdinalIgnoreCase) != -1)
            {
                t = DataBaseObjectType.Procedure;
            }
            else if (path.IndexOf("Tables", StringComparison.OrdinalIgnoreCase) != -1)
            {
                t = DataBaseObjectType.Table;
            }
            else if (path.IndexOf("Views", StringComparison.OrdinalIgnoreCase) != -1)
            {
                t = DataBaseObjectType.View;
            }
            else if (path.IndexOf("Functions", StringComparison.OrdinalIgnoreCase) != -1)
            {
                t = DataBaseObjectType.Function;
            }
            else if (path.IndexOf("Scripts", StringComparison.OrdinalIgnoreCase) != -1)
            {
                t = DataBaseObjectType.Script;
            }
            else if (path.IndexOf("Script", StringComparison.OrdinalIgnoreCase) != -1)
            {
                t = DataBaseObjectType.Script;
            }

            string text = System.IO.File.ReadAllText(path);

            dataBase.Objects.Add(new DataBaseObject
            {
                Name = Path.GetFileName(path).Replace(".sql", string.Empty),
                Path = path,
                Type = t,
                Text = text,
                Date = File.GetLastWriteTime(path)
            });

            // Console.WriteLine("Processed file '{0}'.", path);
        }
    }
}
