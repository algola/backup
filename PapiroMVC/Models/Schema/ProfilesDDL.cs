using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaManagemet;
using System.Data.Objects;
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
            dbS.AddColumnToTable("profile", "Culture", SchemaDb.String, "50");
            dbS.AddColumnToTable("profile", "Phone", SchemaDb.String, "50");
            dbS.AddColumnToTable("profile", "VatNumber", SchemaDb.String, "50");
            dbS.AddColumnToTable("profile", "TaxCode", SchemaDb.String, "50");
            dbS.AddColumnToTable("profile", "test", SchemaDb.String, "50");

            dbS.AddColumnToTable("profile", "Target", SchemaDb.String, "255");
            dbS.AddColumnToTable("profile", "Last3Dgt", SchemaDb.String, "3");
            dbS.AddColumnToTable("profile", "ExpiredM", SchemaDb.String, "2");
            dbS.AddColumnToTable("profile", "ExpiredY", SchemaDb.String, "4");

        }
    }
}
