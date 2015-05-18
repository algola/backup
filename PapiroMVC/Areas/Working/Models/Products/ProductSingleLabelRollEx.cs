using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Novacode;

namespace PapiroMVC.Models
{

    [KnownType(typeof(ProductSingleLabelRoll))]
    [MetadataType(typeof(ProductSingleLabelRoll_MetaData))]
    public partial class ProductSingleLabelRoll : Product
    {

        public ProductSingleLabelRoll()
        {
            this.TypeOfProduct = ProductType.ProductSingleLabelRoll;
        }

        public override void InitProduct()
        {
            base.InitProduct();

            var p = new ProductPartLabelRollArticle();
            var part = new ProductPartSingleLabelRoll();

            part.DCut = DCut;

            part.DCut1 = DCut1;
            part.DCut2 = DCut2;

            part.ShowDCut = ShowDCut;
            part.IsDCut = true;

            //max and min interspace
            part.HaveDCutLimit = true;
            part.MaxDCut = 0.6;
            part.MinDCut = 0.2;
            part.TypeOfDCut1 = 0;

            if (CodMenuProduct == "EtichetteRotolo")
            {
                // 0 = Quadrato // 1 = Ovale // 2 = Sagomato
                part.FormatType = 0; //quadrata 
            }

            if (CodMenuProduct == "EtichetteSagRotolo")
            {
                // 0 = Quadrato // 1 = Ovale // 2 = Sagomato
                part.FormatType = 2; //sagomata 
            }

            if (CodMenuProduct == "FasceGommateRotolo")
            {
                part.HaveDCutLimit = true;
                part.MaxDCut = 0;
                part.MinDCut = 0;
                part.TypeOfDCut1 = 2; // must be 0;   
                part.FormatType = -1; //nessuna fustella 
            }

            part.ProductPartTasks = this.GetInitalizedPartTask();

            ProductPartTask partTask;
            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "STAMPAETICHROTOLO_NO");
            partTask.Hidden = false;
            partTask.ImplantHidden = false;
            partTask.IndexOf = 10;

            partTask.CodItemGraph = "ST";

            //partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "VERNICIATURA_NO");
            //partTask.Hidden = false;
            //partTask.IndexOf = 2;

            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "FUSTELLATURAROTOLO_NO");
            partTask.Hidden = true;
            partTask.ImplantHidden = false;
            partTask.IndexOf = 20;

            partTask.CodItemGraph = "FS";

            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "STAMPAACALDOROTOLO_NO");
            partTask.Hidden = false;
            partTask.ImplantHidden = null; //impant is visibile only if task is visibile
            partTask.IndexOf = 30;

            partTask.CodItemGraph = "SC";

            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "SERIGRAFIAROTOLO_NO");
            partTask.Hidden = false;
            partTask.ImplantHidden = false; //impant is visibile only if task is visibile
            partTask.IndexOf = 35;

            partTask.CodItemGraph = "SE";

            //if this is selected we have to reset tv task
        //    partTask.IfSelectedResetOtherCodItemGraph = "TV";

            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "TAVOLOCONTROLLO_SI");
            partTask.Hidden = true;
            partTask.ImplantHidden = true;
            partTask.IndexOf = 40;

            partTask.CodItemGraph = "TV";

            part.ProductPartPrintableArticles.Add(p);
            ProductParts.Add(part);

            //grafo diretto del prodotto
            ProductGraphLinks.Clear();

            ProductGraphLinks.Add(new ProductGraphLink { CodItemGraph = "ST", CodItemGraphLink = "FS" });
            ProductGraphLinks.Add(new ProductGraphLink { CodItemGraph = "FS", CodItemGraphLink = "SC" });
            ProductGraphLinks.Add(new ProductGraphLink { CodItemGraph = "SC", CodItemGraphLink = "SE" });
            ProductGraphLinks.Add(new ProductGraphLink { CodItemGraph = "SE", CodItemGraphLink = "TV" });

        }

        public override List<ProductPartTask> GetInitalizedPartTask()
        {
            var tsksInPart = new List<ProductPartTask>();

            ProductPartTask pt;

            String[] codTypeOfTasks = { "STAMPAETICHROTOLO", "FUSTELLATURAROTOLO", "STAMPAACALDOROTOLO",
                                          "SERIGRAFIAROTOLO", 
                                          "TAVOLOCONTROLLO" };

            foreach (var item in codTypeOfTasks)
            {

                if (item == "SERIGRAFIAROTOLO")
                {
                    pt = new ProductPartSerigraphy();
                }
                else
                {
                    pt = new ProductPartTask();
                }

                //default selection
                pt.OptionTypeOfTask = SystemTaskList.FirstOrDefault(x => x.CodTypeOfTask == item).OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item + "_NO");

                if (item == "TAVOLOCONTROLLO")
                    pt.OptionTypeOfTask = SystemTaskList.FirstOrDefault(x => x.CodTypeOfTask == item).OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item + "_SI");

                pt.CodOptionTypeOfTask = pt.OptionTypeOfTask.CodOptionTypeOfTask;
                pt.Hidden = true;
                tsksInPart.Add(pt);
            }

            return tsksInPart;
        }

        public override string ToString()
        {
            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProduct);
            var s = (string)t.GetProperty("CodMenuProduct" + this.CodMenuProduct).GetValue(null, null);

            return s + " " + base.ToString();
        }

        public override void MergeField(DocX doc)
        {
            base.MergeField(doc);
        }

    }
}
