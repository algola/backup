using SchemaManagemet;
using System.Data.Entity;

namespace TestSchemaManagement.Model
{    
    class CustomerSupplierDLL :IDDL
    {
        public void UpdateSchema(DbContext ctx)
        {
            var dbS = new SchemaDb();
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
            dbS.AddTable("CustomerSuppliers");
            dbS.AddColumnToTable("CustomerSuppliers", "CodCustomerSupplier", SchemaDb.StringPK, "70");
            dbS.AddColumnToTable("CustomerSuppliers", "Selector", SchemaDb.Int, "0");
            dbS.AddColumnToTable("CustomerSuppliers", "BusinessName", SchemaDb.String, "255");
            dbS.AddColumnToTable("CustomerSuppliers", "VatNumber", SchemaDb.String, "50");

            dbS.AddColumnToTable("CustomerSuppliers", "TaxCode", SchemaDb.String, "50");
            dbS.AddColumnToTable("CustomerSuppliers", "Outdated", SchemaDb.Bool, "0");           

            dbS.AddTable("CustomerSupplierBases");
            dbS.AddColumnToTable("CustomerSupplierBases", "CodCustomerSupplier", SchemaDb.String, "70");
            dbS.AddColumnToTable("CustomerSupplierBases", "CodCustomerSupplierBase", SchemaDb.StringPK, "100");

            dbS.AddColumnToTable("CustomerSupplierBases", "CodTypeOfBase", SchemaDb.String, "50");
            dbS.AddColumnToTable("CustomerSupplierBases", "Address", SchemaDb.String, "255");
            dbS.AddColumnToTable("CustomerSupplierBases", "City", SchemaDb.String, "255");
            dbS.AddColumnToTable("CustomerSupplierBases", "Province", SchemaDb.String, "255");
            dbS.AddColumnToTable("CustomerSupplierBases", "PostalCode", SchemaDb.String, "10");
            dbS.AddColumnToTable("CustomerSupplierBases", "Country", SchemaDb.String, "250");
            dbS.AddColumnToTable("CustomerSupplierBases", "Phone", SchemaDb.String, "50");
            dbS.AddColumnToTable("CustomerSupplierBases", "Fax", SchemaDb.String, "50");
            dbS.AddColumnToTable("CustomerSupplierBases", "Email", SchemaDb.String, "255");
            dbS.AddColumnToTable("CustomerSupplierBases", "Note", SchemaDb.Memo, "0");
            dbS.AddColumnToTable("CustomerSupplierBases", "Referee", SchemaDb.String, "200");
            dbS.AddColumnToTable("CustomerSupplierBases", "Pec", SchemaDb.String, "255");

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

            dbS.AddForeignKey("CustomerSupplierBases", "CodCustomerSupplier", "CustomerSuppliers", "CodCustomerSupplier");

            //TypeOfBase              TypeOfBase
            //NomeTypeOfBase            BaseName

            dbS.AddTable("TypeOfBase");
            dbS.AddColumnToTable("TypeOfBase", "CodTypeOfBase", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("TypeOfBase", "BaseName", SchemaDb.String, "100");

            dbS.AddForeignKey("CustomerSupplierBases", "CodTypeOfBase", "TypeOfBase", "CodTypeOfBase");

        }
    }
}
