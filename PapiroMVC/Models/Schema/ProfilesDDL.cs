using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class ProfilesDDL :IDDL
    {
        SchemaDb dbS;

        public ProfilesDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {

            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("profile");
            dbS.AddColumnToTable("profile", "Name", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("profile", "CompanyName", SchemaDb.String, "50");
            dbS.AddColumnToTable("profile", "Base", SchemaDb.String, "255");
            dbS.AddColumnToTable("profile", "City", SchemaDb.String, "255");
            dbS.AddColumnToTable("profile", "Culture", SchemaDb.String, "50");
            dbS.AddColumnToTable("profile", "Phone", SchemaDb.String, "50");
            dbS.AddColumnToTable("profile", "VatNumber", SchemaDb.String, "50");
            dbS.AddColumnToTable("profile", "TaxCode", SchemaDb.String, "50");
            dbS.AddColumnToTable("profile", "test", SchemaDb.String, "50");

            //dbS.AddColumnToTable("profile", "BrianTreeCustomerId", SchemaDb.String, "255");
            dbS.AddColumnToTable("profile", "BrainTreeToken", SchemaDb.String, "255");


            //---------------------------------------------------------------------------------------------

            //Second Table
            dbS.AddTable("modules");
            dbS.AddColumnToTable("modules", "CodModuleName", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("modules", "Name", SchemaDb.String, "50");
            dbS.AddForeignKey("modules", "Name", "profile", "Name");


            dbS.AddColumnToTable("modules", "CodModule", SchemaDb.String, "50");

            dbS.AddColumnToTable("modules", "ActivationDate", SchemaDb.Date, "0");
            dbS.AddColumnToTable("modules", "ExpirationDate", SchemaDb.Date, "0");
            dbS.AddColumnToTable("modules", "MontlyPrice", SchemaDb.String, "50");
            dbS.AddColumnToTable("modules", "Discount", SchemaDb.Double, "0");
            dbS.AddColumnToTable("modules", "Status", SchemaDb.Int, "0");

            dbS.AddColumnToTable("modules", "Users", SchemaDb.Int, "0");
            dbS.AddColumnToTable("modules", "PermaLink", SchemaDb.String, "255");

            dbS.AddColumnToTable("modules", "IndexOf", SchemaDb.Int, "0");

            //---------------------------------------------------------------------------------------------

            //Second Table
            dbS.AddTable("orders");
            dbS.AddColumnToTable("orders", "CodOrder", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("orders", "Name", SchemaDb.String, "50");
            dbS.AddForeignKey("orders", "Name", "profile", "Name");

            dbS.AddColumnToTable("orders", "OrderDate", SchemaDb.Date, "0");
            dbS.AddColumnToTable("orders", "Status", SchemaDb.Int, "0");
            dbS.AddColumnToTable("orders", "Price", SchemaDb.String, "50");
            dbS.AddColumnToTable("orders", "Discount", SchemaDb.Double, "0");
            dbS.AddColumnToTable("orders", "Total", SchemaDb.String, "50");

            //---------------------------------------------------------------------------------------------

            //Second Table
            dbS.AddTable("orderRows");
            dbS.AddColumnToTable("orderRows", "CodOrderRows", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("orderRows", "CodModuleName", SchemaDb.String, "50");
            dbS.AddForeignKey("orderRows", "CodModuleName", "modules", "CodModuleName");
            dbS.AddColumnToTable("orderRows", "CodOrder", SchemaDb.String, "50");
            dbS.AddForeignKey("orderRows", "CodOrder", "Orders", "CodOrder");

            dbS.AddColumnToTable("orderRows", "Description", SchemaDb.String, "255");
            dbS.AddColumnToTable("orderRows", "MontlyPrice", SchemaDb.String, "50");
            dbS.AddColumnToTable("orderRows", "Months", SchemaDb.Int, "0");
            dbS.AddColumnToTable("orderRows", "Discount", SchemaDb.String, "50");
            dbS.AddColumnToTable("orderRows", "Total", SchemaDb.String, "50");



        }
    }
}
