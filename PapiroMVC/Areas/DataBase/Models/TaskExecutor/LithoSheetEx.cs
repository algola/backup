using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(LithoSheet_MetaData))]
    public partial class LithoSheet : Litho
    {

        public override CostDetail.QuantityType TypeOfImplantQuantity
        {
            get
            {

                CostDetail.QuantityType ret = CostDetail.QuantityType.NumberTypeOfQuantity;
                return ret;
            }
        }

        public override double GetStarts(string codOptionTypeOfTask)
        {
            double cToPrintF = 0;
            double cToPrintR = 0;
            double cToPrintT = 0;

            switch (codOptionTypeOfTask)
            {
                //4 colori offset fronte e retro
                case "STAMPAOFF_FR_COL":
                    cToPrintF = 4;
                    cToPrintR = 4;
                    break;

                case "STAMPAOFF_FR_BN":
                    cToPrintF = 1;
                    cToPrintR = 1;
                    break;

                case "STAMPAOFF_FRONTE_COL":
                    cToPrintF = 4;
                    cToPrintR = 0;
                    break;

                case "STAMPAOFF_FRONTE_BN":
                    cToPrintF = 1;
                    cToPrintR = 0;
                    break;

                case "STAMPAOFFeDIGITALE_FR_COL":
                    cToPrintF = 4;
                    cToPrintR = 4;
                    break;

                case "STAMPAOFFeDIGITALE_FR_BN":
                    cToPrintF = 1;
                    cToPrintR = 1;
                    break;

                case "STAMPAOFFeDIGITALE_FRONTE_COL":
                    cToPrintF = 4;
                    cToPrintR = 0;
                    break;

                case "STAMPAOFFeDIGITALE_FRONTE_BN":
                    cToPrintF = 1;
                    cToPrintR = 0;
                    break;

                default:
                    cToPrintF = 0;
                    cToPrintR = 0;
                    //throw new Exception();
                    break;
            }

            cToPrintT = cToPrintF + cToPrintR;

            //ci sarebbe da distinguere se è un b/v oppure n, per ora no bianca e volta assieme
            return Math.Ceiling(cToPrintT / (double)this.PrintingUnit);
        }

        public override double GetWashes(string codOptionTypeOfTask)
        {
            double cToPrintF = 0;
            double cToPrintR = 0;
            double cToPrintT = 0;

            switch (codOptionTypeOfTask)
            {
                //4 colori offset fronte e retro
                case "STAMPAOFF_FR_COL":
                case "STAMPAOFF_FRONTE_COL":
                case "STAMPAOFFeDIGITALE_FR_COL":
                case "STAMPAOFFeDIGITALE_FRONTE_COL":
                    cToPrintF = 4;
                    cToPrintR = 0;
                    break;

                case "STAMPAOFF_FR_BN":
                case "STAMPAOFF_FRONTE_BN":
                case "STAMPAOFFeDIGITALE_FR_BN":
                case "STAMPAOFFeDIGITALE_FRONTE_BN":
                    cToPrintF = 1;
                    cToPrintR = 0;
                    break;

                default:
                    cToPrintF = 0;
                    cToPrintR = 0;
                    //throw new Exception();
                    break;
            }

            cToPrintT = cToPrintF + cToPrintR;

            //ci sarebbe da distinguere se è un b/v oppure n, per ora no bianca e volta assieme
            return Math.Ceiling(cToPrintT / (double)this.PrintingUnit);
        }



        public LithoSheet()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.LithoSheet;
        }

        #region Added Properties

        #endregion


        public override string GetEditMethod()
        {
            return "EditLithoSheet";
        }
    }
}
