using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Monkey.DataBaseSnitch.ConsoleApp.Models
{
    class DataBase
    {
        public DataBase()
        {
            Objects = new List<DataBaseObject>();
        }

        public List<DataBaseObject> Objects { get; set; }



    }






}
