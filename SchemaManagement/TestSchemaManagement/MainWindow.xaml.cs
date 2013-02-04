using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SchemaManagemet;

namespace TestSchemaManagement
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            using ( Model.dbEntities ctx = new Model.dbEntities())
            {
                var dbS = new SchemaDb();
                dbS.Ctx = ctx;

                //First Table
                dbS.AddTable("PrinterMachines");
                dbS.AddColumnToTable("PrinterMachines", "IdPrinterMachine", SchemaDb.IntPK, "10");
                dbS.AddColumnToTable("PrinterMachines", "PrinterName", SchemaDb.String, "100");
                
                //Index
                dbS.AddIndex("PrinterMachines", "PrinterName");
                dbS.AddColumnToTable("PrinterMachines", "PrinterName", SchemaDb.String, "100");
                dbS.AddColumnToTable("PrinterMachines", "Percent", SchemaDb.Double, "0");

                //Second Table
                dbS.AddTable("SecondTable");
                dbS.AddColumnToTable("SecondTable", "IdSecondTable", SchemaDb.IntPK, "0");

                //Unsigned Int to implement Foreign Key
                dbS.AddColumnToTable("SecondTable", "IdPrinterMachine", SchemaDb.IntUS, "0");
                dbS.AddForeignKey("SecondTable", "IdPrinterMachine", "PrinterMachines", "IdPrinterMachine");

                //Change String length
                dbS.ChangeStringColumnLegth("PrinterMachines", "PrinterName", "160");

            }
        }
    }
}
