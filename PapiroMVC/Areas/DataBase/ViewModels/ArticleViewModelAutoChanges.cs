using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{

    public class ArticleAutoChangesViewModel
    { 
        private RollPrintableArticleAutoChanges rollPrintableArticleAutoChanges;
        private SheetPrintableArticleAutoChanges sheetPrintableArticleAutoChanges;

        public SheetPrintableArticleAutoChanges SheetPrintableArticleAutoChanges
        {
            get
            {
                if (sheetPrintableArticleAutoChanges == null)
                    sheetPrintableArticleAutoChanges = new SheetPrintableArticleAutoChanges();
                return sheetPrintableArticleAutoChanges;
            }
            set
            {
                sheetPrintableArticleAutoChanges = value;
            }
        }

        public RollPrintableArticleAutoChanges RollPrintableArticleAutoChanges
        {
            get 
            { 
                if (rollPrintableArticleAutoChanges==null)
                    rollPrintableArticleAutoChanges = new RollPrintableArticleAutoChanges();
                return rollPrintableArticleAutoChanges;
            }
            set 
            {
                rollPrintableArticleAutoChanges = value;
            }
        }
    }

    [MetadataType(typeof(ArticleAutoChanges_Metadata))]
    abstract public class ArticleAutoChanges
    {
        public string[] Id { get; set; }
        public string SupplierMaker { get; set; }
        public string SupplyerBuy { get; set; }
    }

    [MetadataType(typeof(PrintableAutoChanges_Metadata))]
    abstract public class PrintableAutoChanges : ArticleAutoChanges
    {
        public enum ProcessNoBvType : int
        {
            NoBvNoModify = 0,
            NoBvNo = 1,
            NoBvYes = 2
        }

        public ProcessNoBvType TypeOfNoBvToModify
        {
            get;
            set;
        }
        public double Hand { get; set; }
    }

    [MetadataType(typeof(RollPrintableArticleAutoChanges_Metadata))]
    public class RollPrintableArticleAutoChanges : PrintableAutoChanges
    {
        public string MqForafait { get; set; }
        public string CostPerMl { get; set; }
        public string CostPerMq { get; set; }
    }

    [MetadataType(typeof(SheetPrintableArticleAutoChanges_Metadata))]
    public class SheetPrintableArticleAutoChanges : PrintableAutoChanges
    {
        public enum ProcessCostType : int
        {
            AllCost = 0,
            PalletCost = 1,
            PackedCost = 2,
            CuttedCost = 3
        }

        public ProcessCostType TypeOfCostToModify
        {
            get;
            set;
        }

        //Cost
        public string CostPerKg { get; set; }
    }

}