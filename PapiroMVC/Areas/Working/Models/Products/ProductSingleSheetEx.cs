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

    [KnownType(typeof(ProductSingleSheet))]
    [MetadataType(typeof(ProductSingleSheet_MetaData))]
    public partial class ProductSingleSheet : Product, IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public ProductSingleSheet()
        {
            this.TypeOfProduct = ProductType.ProductSingleSheet;
        }

        public override void InitProduct()
        {
            base.InitProduct();

            var p = new ProductPartSheetArticle();
            var part = new ProductPartSingleSheet();

            part.DCut = DCut;

            part.DCut1 = DCut1;
            part.DCut2 = DCut2;

            part.ShowDCut = ShowDCut;
            part.IsDCut = ShowDCut;

            part.ProductPartTasks = this.GetInitalizedPartTask();

            ProductPartTask partTask;

            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "STAMPANEW_NO");
            partTask.Hidden = false;
            partTask.ImplantHidden = false;
            partTask.IndexOf = 10;
            partTask.IndexOf = 1;

            partTask.CodItemGraph = "ST";

            if (CodMenuProduct.Contains("Sago"))
            {
                partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "FUSTELLATURA_SI");
                partTask.Hidden = false;
                partTask.ImplantHidden = null; //impant is visibile only if task is visibile
                partTask.IndexOf = 20;

                partTask.CodItemGraph = "FS";

                partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "TAGLIO_NO");
                partTask.Hidden = true;
                partTask.ImplantHidden = true;
                partTask.IndexOf = 30;

                partTask.CodItemGraph = "TG";

            }
            else
            {
                partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "FUSTELLATURA_NO");
                partTask.Hidden = true;
                partTask.ImplantHidden = null; //impant is visibile only if task is visibile
                partTask.IndexOf = 20;

                partTask.CodItemGraph = "FS";

                partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "TAGLIO_AL_VIVO");
                partTask.Hidden = false;
                partTask.ImplantHidden = true; //impant is visibile only if task is visibile
                partTask.IndexOf = 20;

                partTask.CodItemGraph = "TG";

            }

            part.ProductPartPrintableArticles.Add(p);
            ProductParts.Add(part);

            //grafo diretto del prodotto
            ProductGraphLinks.Clear();

            ProductGraphLinks.Add(new ProductGraphLink { CodItemGraph = "ST", CodItemGraphLink = "FS" });
            ProductGraphLinks.Add(new ProductGraphLink { CodItemGraph = "FS", CodItemGraphLink = "TG" });

        }

        public override List<ProductPartTask> GetInitalizedPartTask()
        {
            var tsksInPart = base.GetInitalizedPartTask();

            ProductPartTask pt;

            String[] codTypeOfTasks = {"FUSTELLATURA","TAGLIO" };

            foreach (var item in codTypeOfTasks)
            {

                pt = new ProductPartTask();
                //default selection
                pt.OptionTypeOfTask = SystemTaskList.FirstOrDefault(x => x.CodTypeOfTask == item).OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item + "_NO");

                if (item == "FUSTELLATURA" && CodMenuProduct.Contains("Sago"))
                    pt.OptionTypeOfTask = SystemTaskList.FirstOrDefault(x => x.CodTypeOfTask == item).OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item + "_SI");

                if (item == "TAGLIO" && !CodMenuProduct.Contains("Sago"))
                    pt.OptionTypeOfTask = SystemTaskList.FirstOrDefault(x => x.CodTypeOfTask == item).OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item + "_AL_VIVO");
                
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

        #region Proprietà aggiuntive




        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
                   //Specify validation property
                       ""
               };

        public string Error
        {
            get
            {
                return null;
            }
        }

        public virtual string this[string proprieta]
        {
            get
            {
                string result = null;
                return result;
            }
        }

        //Check validation of entity
        public virtual bool IsValid
        {
            get
            {
                bool ret = true;
                foreach (string prop in proprietaDaValidare)
                {
                    if (this[prop] != null)
                        ret = false;
                }
                return ret;
            }
        }

        #endregion

        #region Handle copy for modify

        public virtual void Copy(ProductSingleSheet to)
        {
            //All properties of object
            //and pointer of sons


        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductSingleSheet copyOfObject = (ProductSingleSheet)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);

            //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
            //Example
            //ProductPart partCopy = (ProductPart)Activator.CreateInstance(copyOfObject.Prodotto.GetType());
            ////l'oggetto partCopy sarà una copia del contenuto dell'oggetto originale
            //this.Prodotto.Copia(partCopy);

            //sulla copia del prodotto in producto assegno la copia del suo prodotto
            //Example
            //copiaProdottoInProducto.Prodotto = null;
            //copiaProdotto.ProdottoInProducto = null;
            //copiaProdottoInProducto.Prodotto = copiaProdotto;
            //copiaProdotto.ProdottoInProducto.Add(copiaProdottoInProducto);
            //END COPY OF CHILD

            return copyOfObject;
        }

        public void ChildsNull()
        {
            //Set all chied to null 

            //Example
            //this.Prodotto = null;
        }

        #endregion
    }
}
