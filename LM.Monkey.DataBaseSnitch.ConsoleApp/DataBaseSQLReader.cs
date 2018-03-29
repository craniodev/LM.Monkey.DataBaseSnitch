using LM.Monkey.DataBaseSnitch.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LM.Monkey.DataBaseSnitch.ConsoleApp
{
    internal static class DataBaseSQLReader
    {
        public static DataBase Read(string cn)
        {
            var dataBase = new DataBase();
            dataBase.Objects.AddRange(GetObjects(cn));
            dataBase.Objects.AddRange(GetTables(cn));
            return dataBase;
        }

        private static List<DataBaseObject> GetObjects(string cn)
        {
            var procedures = new List<DataBaseObject>();

            using (SqlConnection connection = new SqlConnection(cn))
            {
                SqlCommand command = new SqlCommand(@"
SELECT name
	, definition AS Text
	, type_desc
	,create_date
	,modify_date
    FROM sys.sql_modules m 
INNER JOIN sys.objects o 
        ON m.object_id=o.object_id
WHERE CHARINDEX('sp_', name) <=0 

", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    procedures.Add(new DataBaseObject
                    {
                        Name = reader[0].ToString(),
                        Type = GetType(reader[2].ToString()),
                        Text = reader[1].ToString(),
                        Date = GetGreatDate(reader[3], reader[4])
                    });
                }
                reader.Close();
            }

            return procedures;
        }

        private static DateTime GetGreatDate(object v1, object v2)
        {
            if (Convert.ToDateTime(v1) > Convert.ToDateTime(v2))
            {
                return Convert.ToDateTime(v1);
            }
            else
            {
                return Convert.ToDateTime(v2);
            }
        }

        private static List<DataBaseObject> GetTables(string cn)
        {
            var procedures = new List<DataBaseObject>();

            using (SqlConnection connection = new SqlConnection(cn))
            {
                SqlCommand command = new SqlCommand(SQL_SCRIPT.GET_TABLES, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    procedures.Add(new DataBaseObject
                    {
                        Name = reader[0].ToString(),
                        Type = DataBaseObjectType.Table,
                        Text = GetTableDefinition(cn, reader[0].ToString())
                    });
                }
                reader.Close();
            }

            return procedures;
        }

        private static DataBaseObjectType GetType(string str)
        {
            if (str.IndexOf("PROCEDURE") != -1) return DataBaseObjectType.Procedure;
            if (str.IndexOf("View") != -1) return DataBaseObjectType.View;
            if (str.IndexOf("Function") != -1) return DataBaseObjectType.Function;
            return DataBaseObjectType.NotFound;
        }

        private static string GetTableDefinition(string cn, string tableName)
        {
            var text = string.Empty;

            var procedures = new List<DataBaseObject>();

            using (SqlConnection connection = new SqlConnection(cn))
            {
                SqlCommand command = new SqlCommand(string.Format(SQL_SCRIPT.GET_TABLE_Definition, tableName), connection);
                connection.Open();
                text = command.ExecuteScalar().ToString();
            }

            return text;
        }

    }
}
