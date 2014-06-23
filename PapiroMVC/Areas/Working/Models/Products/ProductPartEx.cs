﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Models.Resources.Products;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{

    [KnownType(typeof(ProductPart))]
    [MetadataType(typeof(ProductPart_MetaData))]
    public partial class ProductPart : ICloneable
    {

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPart copyOfObject = (ProductPart)Activator.CreateInstance(kindOfObject);
            this.Copy(copyOfObject);

            return copyOfObject;
        }

        public virtual void Copy(ProductPart to)
        {
            to.TimeStampTable = this.TimeStampTable;

            to.CodProduct = this.CodProduct;
            to.ProductPartName = this.ProductPartName;
            to.PrintingType = this.PrintingType;
            to.Format = this.Format;
            to.ServicesNumber = this.ServicesNumber;
            //CodProductPart_  = this. ;
            to.FormatOpened = this.FormatOpened;
            to.SubjectNumber = this.SubjectNumber;
            to.DCut = this.DCut;
            to.IsDCut = this.IsDCut;
            to.DCut1 = this.DCut1;
            to.DCut2 = this.DCut2;
            to.SideOnSide = this.SideOnSide;
            to.HaveDCutLimit = this.HaveDCutLimit;
            to.MaxDCut = this.MaxDCut;
            to.MinDCut = this.MinDCut;
            to.TypeOfDCut1 = this.TypeOfDCut1;

        }


        public double AvarageDCut
        { get { return ((MinDCut ?? 0) + (MaxDCut ?? 0)) / 2; } }


        //formato in mm
        public String Formatmm
        {
            get
            {
                if (Format == null)
                {
                    return null;
                }
                else
                {
                    return (Format.GetSide1() * 10).ToString() + "x" + (Format.GetSide2() * 10).ToString();
                }
            }

            set
            {
                try
                {
                    Format = (value.GetSide1() / 10).ToString() + "x" + (value.GetSide2() / 10).ToString();
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }


        public virtual void UpdateOpenedFormat()
        {
            this.FormatOpened = this.Format;
        }

        public bool ShowDCut { get; set; }

        protected List<ProductPartsPrintableArticle> productPartPrintableArticles;
        public List<ProductPartsPrintableArticle> ProductPartsPrintableArticlePerView
        {
            get
            {
                if (productPartPrintableArticles == null)
                {
                    productPartPrintableArticles = this.ProductPartPrintableArticles.ToList();
                }
                return productPartPrintableArticles;
            }

            set
            {
                productPartPrintableArticles = value;
                ProductPartPrintableArticles = productPartPrintableArticles;
            }

        }

        protected List<ProductPartTask> productPartTasks;
        public List<ProductPartTask> ProductPartTasksPerView
        {
            get
            {
                if (productPartTasks == null)
                {
                    productPartTasks = this.ProductPartTasks.ToList();
                }

                return productPartTasks;

            }

            set
            {
                productPartTasks = value;
                ProductPartTasks = productPartTasks;
            }

        }

        #region Proprietà aggiuntive
        public enum ProductPartType : int
        {
            ProductPartSingleSheet = 0,
            ProductPartCoverSheet = 1,
            ProductPartBookSheet = 2,
            ProductPartBlockSheet = 3,
            ProductPartSinglePlotter = 4,

            ProductPartRigid = 5,
            ProductPartSingleLabelRoll = 6,
            ProductPartSoft = 7,
        }

        public ProductPartType TypeOfProductPart
        {
            get;
            protected set;
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

        #endregion

        public bool IsSelected
        {
            get;
            set;
        }

        public override string ToString()
        {
            var ptArt = String.Empty;
            foreach (var item in ProductPartPrintableArticles)
            {
                ptArt += item.ToString() == String.Empty ? "" : item.ToString() + "\n";
            }

            var pTasks = String.Empty;
            foreach (var item in this.ProductPartTasks)
            {
                if (!item.CodOptionTypeOfTask.Contains("_NO"))
                {
                    pTasks += item.ToString() == String.Empty ? "" : item.ToString() + "\n";
                }
            }

            return ptArt + pTasks;
        }

        public virtual void ToName()
        {
            var x = Product.ProductNameGenerator;
            x = x.Replace("%PARTFORMATOPENMMSIDE1", this.Formatmm.GetSide1().ToString());
            x = x.Replace("%PARTFORMATOPENMMSIDE2", this.Formatmm.GetSide2().ToString());
            x = x.Replace("%PARTFORMATOPENMM", this.Formatmm);

            x = x.Replace("%PARTFORMATOPENSIDE1", this.Format.GetSide1().ToString());
            x = x.Replace("%PARTFORMATOPENSIDE2", this.Format.GetSide2().ToString());
            x = x.Replace("%PARTFORMATOPEN", this.Format);

            Product.ProductNameGenerator = x;

            foreach (var item in ProductPartPrintableArticles)
            {
                item.ToName();
            }

            foreach (var item in this.ProductPartTasks)
            {
                if (!item.CodOptionTypeOfTask.Contains("_NO"))
                {
                    item.ToName();
                }
            }
        }

    }
}
