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

            part.ProductPartTasks = this.GetInitalizedPartTask();

            ProductPartTask partTask;
            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "STAMPAETICHROTOLO_NO");
            partTask.Hidden = false;
            partTask.IndexOf = 1;

            part.ProductPartPrintableArticles.Add(p);
            ProductParts.Add(part);

        }

  
        public override List<ProductPartTask> GetInitalizedPartTask()
        {
            var tsksInPart = new List<ProductPartTask>();

            ProductPartTask pt;

            String[] codTypeOfTasks = { "STAMPAETICHROTOLO" };

            foreach (var item in codTypeOfTasks)
            {
                pt = new ProductPartTask();
                //default selection
                pt.OptionTypeOfTask = SystemTaskList.FirstOrDefault(x => x.CodTypeOfTask == item).OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item + "_NO");
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
