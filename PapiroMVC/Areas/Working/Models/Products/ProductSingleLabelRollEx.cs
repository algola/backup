using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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

            if (CodMenuProduct == "FasceGommateRotolo")
            {
                part.HaveDCutLimit = true;
                part.MaxDCut = 0;
                part.MinDCut = 0;
                part.TypeOfDCut1 = 2; // must be 0;                
            }

            part.ProductPartTasks = this.GetInitalizedPartTask();

            ProductPartTask partTask;
            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "STAMPAETICHROTOLO_NO");
            partTask.Hidden = false;
            partTask.ImplantHidden = false;
            partTask.IndexOf = 1;

            partTask.CodItemGraph = "ST";

            //partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "VERNICIATURA_NO");
            //partTask.Hidden = false;
            //partTask.IndexOf = 2;

            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "FUSTELLATURA_NO");
            partTask.Hidden = true;
            partTask.ImplantHidden = false;
            partTask.IndexOf = 2;

            partTask.CodItemGraph = "FS";

            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "TAVOLOCONTROLLO_SI");
            partTask.Hidden = true;
            partTask.ImplantHidden = true;
            partTask.IndexOf = 2;

            partTask.CodItemGraph = "TV";

            part.ProductPartPrintableArticles.Add(p);
            ProductParts.Add(part);

            //grafo diretto del prodotto
            ProductGraphLinks.Clear();

            ProductGraphLinks.Add(new ProductGraphLink { CodItemGraph = "ST", CodItemGraphLink = "FS" });
            ProductGraphLinks.Add(new ProductGraphLink { CodItemGraph = "FS", CodItemGraphLink = "TV" });

        }



  
        

        public override List<ProductPartTask> GetInitalizedPartTask()
        {
            var tsksInPart = new List<ProductPartTask>();

            ProductPartTask pt;

            String[] codTypeOfTasks = { "STAMPAETICHROTOLO", "FUSTELLATURA", "TAVOLOCONTROLLO" };

            foreach (var item in codTypeOfTasks)
            {
                pt = new ProductPartTask();
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

    }
}
