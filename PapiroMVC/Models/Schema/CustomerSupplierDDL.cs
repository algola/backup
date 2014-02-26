using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class CustomerSupplierDLL :IDDL
    {
        SchemaDb dbS;

        public CustomerSupplierDLL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {
            
            dbS.Ctx = ctx;

            //CustomerSuppliers        CustomerSuppliers
            //CodCustomerSuppliers      CodCustomerSupplier
            //Selettore         Selector
            //BusinessName    BusinessName
            //VatNumber              VatNumber
            //TaxCode        TaxCode
            //Outdated          Outdated

            //CustomerSupplierBases              CustomerSupplierBases
            //CodCustomerSupplierBase            CodCustomerSupplierBase

            //First Table
            dbS.AddTable("customersuppliers");
            dbS.AddColumnToTable("customersuppliers", "CodCustomerSupplier", SchemaDb.StringPK, "70");
            dbS.AddColumnToTable("customersuppliers", "Selector", SchemaDb.Int, "0");
            dbS.AddColumnToTable("customersuppliers", "BusinessName", SchemaDb.String, "255");
            dbS.AddColumnToTable("customersuppliers", "VatNumber", SchemaDb.String, "50");

            dbS.AddColumnToTable("customersuppliers", "TaxCode", SchemaDb.String, "50");
            dbS.AddColumnToTable("customersuppliers", "Outdated", SchemaDb.Bool, "0");           

            dbS.AddTable("customersupplierbases");
            dbS.AddColumnToTable("customersupplierbases", "CodCustomerSupplier", SchemaDb.String, "70");
            dbS.AddColumnToTable("customersupplierbases", "CodCustomerSupplierBase", SchemaDb.StringPK, "100");

            dbS.AddColumnToTable("customersupplierbases", "CodTypeOfBase", SchemaDb.String, "50");
            dbS.AddColumnToTable("customersupplierbases", "Address", SchemaDb.String, "255");
            dbS.AddColumnToTable("customersupplierbases", "City", SchemaDb.String, "255");
            dbS.AddColumnToTable("customersupplierbases", "Province", SchemaDb.String, "255");
            dbS.AddColumnToTable("customersupplierbases", "PostalCode", SchemaDb.String, "10");
            dbS.AddColumnToTable("customersupplierbases", "Country", SchemaDb.String, "250");
            dbS.AddColumnToTable("customersupplierbases", "Phone", SchemaDb.String, "50");
            dbS.AddColumnToTable("customersupplierbases", "Fax", SchemaDb.String, "50");
            dbS.AddColumnToTable("customersupplierbases", "Email", SchemaDb.String, "255");
            dbS.AddColumnToTable("customersupplierbases", "Note", SchemaDb.Memo, "0");
            dbS.AddColumnToTable("customersupplierbases", "Referee", SchemaDb.String, "200");
            dbS.AddColumnToTable("customersupplierbases", "Pec", SchemaDb.String, "255");

            //CodTypeOfBase            CodTypeOfBase
            //Address             Address
            //City                 City
            //Province             County
            //PostalCode                   PostalCode
            //Country                 Country
            //Phone              Phone
            //Fax                   Fax
            //Email                 Email
            //Note                  Note
            //Referee           Referee
            //Pec                   Pec

            dbS.AddForeignKey("customersupplierbases", "CodCustomerSupplier", "customersuppliers", "CodCustomerSupplier");

            //TypeOfBase              TypeOfBase
            //NomeTypeOfBase            BaseName

            dbS.AddTable("typeofbase");
            dbS.AddColumnToTable("typeofbase", "CodTypeOfBase", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("typeofbase", "BaseName", SchemaDb.String, "100");

            dbS.AddForeignKey("customersupplierbases", "CodTypeOfBase", "typeofbase", "CodTypeOfBase");

        }
    }
}
