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
        public static string LongString = "longstring";
        public static string StringPK = "stringPK";
        public static string Time = "time";
        public static string Date = "date";
        public static string Decimal = "decimal";

        public string DatabaseName { get; set; }

        public DbContext Ctx
        {
            get;
            set;
        }

        public void CreateDatabase()
        {
            var sql = "CALL AddDatabase('param1');";
            sql = sql.Replace("param1", DatabaseName);
            Ctx.Database.ExecuteSqlCommand(sql);
        }

        /// <summary>
        /// Add a table to Context
        /// </summary>
        /// <param name="name"></param>
        public void AddTable(string name)
        {
            using (var dbTrans = Ctx.Database.BeginTransaction())
            {
                var sql = "CALL AddTable('param1','param2');";
                sql = sql.Replace("param2", name);
                sql = sql.Replace("param1", DatabaseName);
                Ctx.Database.ExecuteSqlCommand(sql);

                dbTrans.Commit();
            }
            
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
            var sql = "CALL AddColumnToTable('param0','param1','param2','param3',param4);";
            sql = sql.Replace("param0", DatabaseName);
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
            var sql = "CALL AddForeignKey('param0','param1','param2','param3','param4');";
            sql = sql.Replace("param0", DatabaseName);            
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
            var sql = "CALL AddIndex('param0','param1','param2','param3');";
            sql = sql.Replace("param0", DatabaseName);
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
            var sql = "CALL ChangeStringColumnLegth('param0','param1','param2', param3);";
            sql = sql.Replace("param0", DatabaseName);
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


//        

        /// <summary>
        /// Change column to Bool not nullable
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        public void ChangeToBoolNotNullable(string tableName, string columnName)
        {
            var sql = "CALL ChangeToBoolNotNullable('param0','param1','param2');";
            sql = sql.Replace("param0", DatabaseName);
            sql = sql.Replace("param1", tableName);
            sql = sql.Replace("param2", columnName);
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
        /// Change column to Double
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        public void ChangeColumnToDouble(string tableName, string columnName)
        {
            var sql = "CALL ChangeColumnToDouble('param0','param1','param2');";
            sql = sql.Replace("param0", DatabaseName);
            sql = sql.Replace("param1", tableName);
            sql = sql.Replace("param2", columnName);
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
        /// Change column to Double
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        public void ChangeColumnToString(string tableName, string columnName)
        {
            var sql = "CALL ChangeColumnToString('param0','param1','param2');";
            sql = sql.Replace("param0", DatabaseName);
            sql = sql.Replace("param1", tableName);
            sql = sql.Replace("param2", columnName);
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
