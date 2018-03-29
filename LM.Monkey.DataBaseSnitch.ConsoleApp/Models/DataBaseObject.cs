using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Monkey.DataBaseSnitch.ConsoleApp.Models
{
    class DataBaseObject
    {
        public string Name { get; set; }
        public DataBaseObjectType Type { get; set; }
        public string Path { get; internal set; }
        public string Text { get; internal set; }
        public DateTime Date { get; internal set; }
    }
}
