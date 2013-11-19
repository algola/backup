using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{    
    [MetadataType(typeof(ArticleCost_MetaData))]
    public abstract partial class ArticleCost :  ICloneable, IDeleteRelated
    {
        #region Proprietà aggiuntive       
        public enum ArticleCostType : int
        {
            SheetPrintableArticlePakedCost = 0,
            SheetPrintableArticlePalletCost = 1,
            SheetPrintableArticleCuttedCost = 2,
            RollPrintableArticleStandardCost = 3,
            RollPrintableArticleCuttedCost = 4,
            RigidPrintableArticleStandardCost = 5,
            ObjectPrintableArticleStandardCost = 6

        }

        public ArticleCostType TypeOfArticleCost
        {
            get;
            protected set;
        }

        #endregion

        public bool IsSelected
        {
            get;
            set;
        }

        #region Handle copy for modify

        public virtual void Copy(ArticleCost to)
        {
            //All properties of object
            //and pointer of sons

            to.CodArticleCost = this.CodArticleCost;
            to.TimeStampTable = this.TimeStampTable;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ArticleCost copyOfObject = (ArticleCost)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);

            //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
            //Example
            //Prodotto copiaProdotto = (Prodotto)Activator.CreateInstance(copiaProdottoInDocumento.Prodotto.GetType());
            ////l'oggetto copisa sarà una copia del contenuto dell'oggetto originale
            //this.Prodotto.Copia(copiaProdotto);

            //sulla copia del prodotto in documento assegno la copia del suo prodotto
            //Example
            //copiaProdottoInDocumento.Prodotto = null;
            //copiaProdotto.ProdottoInDocumento = null;
            //copiaProdottoInDocumento.Prodotto = copiaProdotto;
            //copiaProdotto.ProdottoInDocumento.Add(copiaProdottoInDocumento);
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
