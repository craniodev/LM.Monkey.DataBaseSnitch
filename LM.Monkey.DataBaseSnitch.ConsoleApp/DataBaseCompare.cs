using LM.Monkey.DataBaseSnitch.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Monkey.DataBaseSnitch.ConsoleApp
{
    internal static class DataBaseCompare
    {
        public static Dictionary<string, string> Compare(DataBase source, DataBase target, bool compareScript = false)
        {
            if (!compareScript)
            {
                source.Objects = source.Objects.Where(x => x.Type != DataBaseObjectType.Script).ToList();
                target.Objects = target.Objects.Where(x => x.Type != DataBaseObjectType.Script).ToList();
            }

            var retorno = new Dictionary<string, string>();

            foreach (var s in source.Objects)
            {
                var targetObject = target.Objects.FirstOrDefault(o => o.Name == s.Name);

                if (targetObject == null)
                {
                    AddIndex(retorno, s.Name, "not found");
                }
                else
                {
                    var sourceText = RemoveSpecialCharacters(s.Text);
                    var targetText = RemoveSpecialCharacters(targetObject.Text);

                    sourceText = RemoveLixo(sourceText);
                    targetText = RemoveLixo(targetText);

                    if (!string.Equals(sourceText, targetText, StringComparison.OrdinalIgnoreCase))
                    {
                        var sus = string.Empty;
                        if (s.Date > targetObject.Date)
                        {
                            sus = "(better)";
                        }
                        else
                        {
                            sus = "(old)";
                        }



                        AddIndex(retorno, s.Name, "different" + " hint:" + sus);
                    }
                }
            }

            return retorno;
        }

        private static string RemoveLixo(string sourceText)
        {
            var ini = sourceText.IndexOf("CONSTRAINT");

            if (ini != -1)
            {
                sourceText = sourceText.Remove(ini, sourceText.Length - ini);
            }

            return sourceText;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static void AddIndex(Dictionary<string, string> retorno, string key, string value)
        {
            if (retorno.ContainsKey(key)) return;
            retorno.Add(key, value);


        }

    }
}
