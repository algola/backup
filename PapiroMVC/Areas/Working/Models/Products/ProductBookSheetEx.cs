using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    
    [KnownType(typeof(ProductBookSheet))]
    [MetadataType(typeof(ProductBookSheet_MetaData))]
    public partial class ProductBookSheet : Product, IDataErrorInfo, ICloneable, IDeleteRelated
    {


        public ProductBookSheet()
        {
            this.TypeOfProduct = ProductType.ProductBookSheet;
        }

        /// <summary>
        /// Formato peronalizzato, usato quando l'utente non trova il formato giusto nel DownBox
        /// </summary>
        protected String formatPersonalized;
        public String FormatPersonalized
        {
            get
            {
                return formatPersonalized;
            }
            set
            {
                formatPersonalized = value;

                if (formatPersonalized != null && formatPersonalized != String.Empty)
                {
                    this.Format = formatPersonalized;
                }
            }
        }


        public override void InitProduct()
        {
            base.InitProduct();

            var cover = new ProductPartCoverSheet();
            cover.ProductPartTasks = this.GetInitalizedPartTask();
            cover.ProductPartName = "";
            cover.WidthWings = 0;
            cover.Back = 0;

            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProductPart);
            cover.ProductPartName = (string)t.GetProperty(cover.TypeOfProductPart.ToString()).GetValue(null, null);

            ProductPartTask partTask;
            partTask = cover.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "STAMPAOFFeDIGITALE_NO");
            partTask.Hidden = false;
            partTask.IndexOf = 1;

            partTask = cover.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "PLASTIFICATURA_NO");
            partTask.Hidden = false;
            partTask.IndexOf = 2;

            ProductPartSheetArticle material = new ProductPartSheetArticle();

            cover.ProductPartPrintableArticles.Add(material);
            ProductParts.Add(cover);

            //-------------------------------------------------------------------------------------------

            var intern = new ProductPartBookSheet();
            intern.ProductPartTasks = this.GetInitalizedPartTask();

            partTask = intern.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "STAMPAOFFeDIGITALE_NO");
            partTask.Hidden = false;
            partTask.IndexOf = 1;

            material = new ProductPartSheetArticle();

            intern.ProductPartPrintableArticles.Add(material);
            ProductParts.Add(intern);

            //descrizone che poi andrà nei costi per identificare la parte!
            intern.ProductPartName = (string)t.GetProperty(intern.TypeOfProductPart.ToString()).GetValue(null, null);
            intern.ProductPartName += " 1";

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

        public virtual void Copy(ProductBookSheet to)
        {
            //All properties of object
            //and pointer of sons
            to.Format = this.Format;
        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductBookSheet copyOfObject = (ProductBookSheet)Activator.CreateInstance(kindOfObject);
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


        public override void ProductCodeRigen()
        {
            //parti del prodotto
            var ppart = this.ProductParts.ToList();
            foreach (var item in this.ProductParts)
            {
                item.Format = this.Format;
            }
            base.ProductCodeRigen();
        }

    }
}
