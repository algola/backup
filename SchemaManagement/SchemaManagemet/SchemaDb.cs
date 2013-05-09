using System;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Resources;

namespace SchemaManagemet
{
    /// <summary>
    /// Schema management class
    /// </summary>
    public class SchemaDb
    {
        public static string Bool="Bool";
        public static string Byte = "byte";
        public static string ByteUS = "byteUS";
        public static string Double = "double";
        public static string Int = "int";
        public static string IntPK = "intPK";
        public static string IntUS = "intUS";
        public static string Long = "long";
        public static string Memo = "memo";
        public static string String = "string";
        public static string StringPK = "stringPK";
        public static string Time = "time";

        public DbContext Ctx
        {
            get;
            set;
        }

        public void InitStoredMySql(string sqlfile)
        {
            String sql = "";

            try
            {
                using (StreamReader sr = new StreamReader(sqlfile))
                {
                    sql = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            try
            {
                Ctx.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }        
        }

        /// <summary>
        /// Add a table to Context
        /// </summary>
        /// <param name="name"></param>
        public void AddTable(string name)
        {
            var sql = "CALL AddTable('param1');";
            sql = sql.Replace("param1", name);
            Ctx.Database.ExecuteSqlCommand(sql);
        }

        /// <summary>
        /// Add Column To Table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ColumnName"></param>
        /// <param name="ColumnType"></param>
        /// <param name="ColumnLenght"></param>
        public void AddColumnToTable(string tableName, string ColumnName, string ColumnType, string ColumnLenght)
        {
            var sql = "CALL AddColumnToTable('param1','param2','param3',param4);";
            sql = sql.Replace("param1", tableName);
            sql = sql.Replace("param2", ColumnName);
            sql = sql.Replace("param3", ColumnType);
            sql = sql.Replace("param4", ColumnLenght);
            try
            {
                Ctx.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Add Foreign Key
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ColumnName"></param>
        /// <param name="tableName2"></param>
        /// <param name="ColumnName2"></param>
        public void AddForeignKey(string tableName, string ColumnName, string tableName2, string ColumnName2)
        {
            var sql = "CALL AddForeignKey('param1','param2','param3','param4');";
            sql = sql.Replace("param1", tableName);
            sql = sql.Replace("param2", ColumnName);
            sql = sql.Replace("param3", tableName2);
            sql = sql.Replace("param4", ColumnName2);
            try
            {
                Ctx.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Add Index to Table
        /// Use column,column2,etc.. to specify more than one column
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="ColumnName"></param>
        public void AddIndex(string tableName, string ColumnName)
        {
            var sql = "CALL AddIndex('param1','param2','param3');";
            sql = sql.Replace("param1", tableName);
            sql = sql.Replace("param3", ColumnName);
            sql = sql.Replace("param2", ColumnName.Replace(",", "_"));
            try
            {
                Ctx.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Change string length 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="columnLenght"></param>
        public void ChangeStringColumnLegth(string tableName, string columnName, string columnLenght)
        {
            var sql = "CALL ChangeStringColumnLegth('param1','param2', param3);";
            sql = sql.Replace("param1", tableName);
            sql = sql.Replace("param2", columnName);
            sql = sql.Replace("param3", columnLenght);
            try
            {
                Ctx.Database.ExecuteSqlCommand(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
