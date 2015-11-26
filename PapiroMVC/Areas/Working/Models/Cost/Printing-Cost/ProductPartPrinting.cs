using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    /// <summary>
    /// Get PrintingFormat and calculating gain on this Format based on specifit type
    /// </summary>
    public partial class ProductPartPrinting : ICloneable, IPrintDocX
    {
        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartPrinting copyOfObject = (ProductPartPrinting)Activator.CreateInstance(kindOfObject);
            this.Copy(copyOfObject);

            return copyOfObject;
        }


        public virtual void Copy(ProductPartPrinting to)
        {
            to.TimeStampTable = this.TimeStampTable;
            to.CodProductPartPrinting = null;
            to.PrintingFormat = this.PrintingFormat;

            to.CodProductPart = this.CodProductPart;
            //to.Part = this.Part;

            CostDetail = null;

            var x = (ProductPartPrintingGain)GainPartOnPrinting.Clone();
            x.ProductPartPrinting = to;
            x.CodProductPartPrinting = "";
            to.GainPartOnPrinting = x;

        }



        public enum ProductPartPrintingType : int
        {
           
            ProductPartCoverSheetPrinting = 1,
            ProductPartBookSheetPrinting = 2,
            
            ProductPartDoubleSheetPrinting=8,
            ProductPartRigidPrinting = 5,
            ProductPartSingleSheetPrinting = 0,
            ProductPartSoftPrinting=3,
        }

        public ProductPartPrintingType TypeOfProductPartPrinting
        {
            get;
            set;
        }

        public int MaxGain1 { get; set; }
        public int MaxGain2 { get; set; }

        public int MinGain1 { get; set; }
        public int MinGain2 { get; set; }

        //prova
        public int ForceSide { get; set; }

        public bool AutoCutParameter { get; set; }
        public bool LateralMinDCut { get; set; }

        //    public virtual ProductPartPrintingGain GainPartOnPrinting { get; set; }

        public virtual void Update()
        {

        }

        public ProductPartPrintingGain GainPartOnPrinting
        {
            get
            {
                return GainPartOnPrintings.FirstOrDefault();
            }
            set
            {
                GainPartOnPrintings.Clear();
                GainPartOnPrintings.Add(value);
            }
        }

        public virtual double CalculatedStarts
        {
            get
            {
                if (GainPartOnPrinting != null)
                {
                    return GainPartOnPrinting.Makereadies.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        public virtual double CalculatedGain
        {
            get
            {
                if (GainPartOnPrinting != null)
                {
                    return (double)GainPartOnPrinting.Makereadies.Average(x => x.CalculatedGain ?? 1);
                }
                else
                {
                    return 1;
                }
            }
        }

        public virtual double CalculatedMq
        {
            get
            {
                string format = Part.FormatOpened;
                if (format == null || format == "")
                {
                    format = Part.Format;
                }

                return format.GetSide1() * format.GetSide2() / 10000 * (Part.SubjectNumber ?? 1);
            }
        }

        /// <summary>
        /// Ml in stampa del formato di stampa
        /// </summary>
        public virtual double CalculatedMl
        {
            get
            {
                return PrintingFormat.GetSide2() / 100;
            }
        }

        public virtual double CalculatedMqPrintingFormat
        {
            get
            {
                //       Console.WriteLine(Part);
                string format = PrintingFormat;
                return format.GetSide1() * format.GetSide2() / 10000;
            }
        }


        public virtual bool CalculatedSideOnSide
        {
            get
            {
                return GainPartOnPrinting.Makereadies.FirstOrDefault().SideOnSide ?? false;
            }
        }

        public virtual int CalculatedSide1Gain
        {
            get
            {
                return GainPartOnPrinting.Makereadies.FirstOrDefault().ShapeOnSide1 ?? 0;
            }
        }

        public virtual int CalculatedSide2Gain
        {
            get
            {
                return GainPartOnPrinting.Makereadies.FirstOrDefault().ShapeOnSide2 ?? 0;
            }
        }

        public virtual double CalculatedDCut1
        {
            get
            {
                return GainPartOnPrinting.DCut1 ?? 0;
            }
        }

        public virtual double CalculatedDCut2
        {
            get
            {
                return GainPartOnPrinting.DCut2 ?? 0;
            }
        }

        public virtual void MergeField(DocX doc)
        {

            //cerco di capire se la macchina èflexo... se è flexo voglio stampare lo zeta e lo h
            if (CostDetail.TaskexEcutorSelected.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo)
            {
                var z = ((Flexo)CostDetail.TaskexEcutorSelected).GetZFromCm(PrintingFormat.GetSide2());
                var pFtoFlexo = "h" + PrintingFormat.GetSide1() + " z" + z.ToString();
                doc.AddCustomProperty(new Novacode.CustomProperty("PPP.PrintingFormat", pFtoFlexo));
            }
            else
            {
                doc.AddCustomProperty(new Novacode.CustomProperty("PPP.PrintingFormat", this.PrintingFormat));
            }

            doc.AddCustomProperty(new Novacode.CustomProperty("PPP.DCut1", this.CalculatedDCut1));
            doc.AddCustomProperty(new Novacode.CustomProperty("PPP.DCut2", this.CalculatedDCut2));
            doc.AddCustomProperty(new Novacode.CustomProperty("PPP.Gain1", this.CalculatedSide1Gain));
            doc.AddCustomProperty(new Novacode.CustomProperty("PPP.Gain2", this.CalculatedSide2Gain));


            if (Part != null)
            {
                doc.AddCustomProperty(new Novacode.CustomProperty("PPP.FormatOpened", this.Part.FormatOpened));
                doc.AddCustomProperty(new Novacode.CustomProperty("PPP.Format", this.Part.Format));                
            }
            else
            {
                doc.AddCustomProperty(new Novacode.CustomProperty("PPP.FormatOpened", ""));
                doc.AddCustomProperty(new Novacode.CustomProperty("PPP.Format", this.Part.Format));
            }


        }


    }
}