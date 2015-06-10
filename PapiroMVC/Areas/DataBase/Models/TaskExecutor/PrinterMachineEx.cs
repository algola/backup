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
    [MetadataType(typeof(PrinterMachine_MetaData))]
    public abstract partial class PrinterMachine : TaskExecutor
    {
       
        #region Proprietà aggiuntive

        #endregion


        //get number of impants by type of task!! ex: 2 colors --> 2 implants
        public override double GetImplants(string codOptionTypeOfTask)
        {

            var x =GetColorFR(codOptionTypeOfTask);

            //Starts is used with printerFormat to have
            return Math.Ceiling(x.cToPrintT);
        }



        public override PrintingColor GetColorFR(string codOptionTypeOfTask)
        {
            var ret = new PrintingColor();
            switch (codOptionTypeOfTask)
            {

                //4 colori offset fronte e retro
                case "STAMPAETICHROTOLO_1":
                    ret.cToPrintF = 1;
                    ret.cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_1RETRO":
                    ret.cToPrintF = 1;
                    ret.cToPrintR = 1;
                    break;

                case "STAMPAETICHROTOLO_2":
                    ret.cToPrintF = 2;
                    ret.cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_2RETRO":
                    ret.cToPrintF = 2;
                    ret.cToPrintR = 1;
                    break;

                case "STAMPAETICHROTOLO_3":
                    ret.cToPrintF = 3;
                    ret.cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_3RETRO":
                    ret.cToPrintF = 3;
                    ret.cToPrintR = 1;
                    break;

                case "STAMPAETICHROTOLO_4":
                    ret.cToPrintF = 4;
                    ret.cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_4RETRO":
                    ret.cToPrintF = 4;
                    ret.cToPrintR = 1;
                    break;

                case "STAMPAETICHROTOLO_5":
                    ret.cToPrintF = 5;
                    ret.cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_5RETRO":
                    ret.cToPrintF = 5;
                    ret.cToPrintR = 1;
                    break;

                case "STAMPAETICHROTOLO_6":
                    ret.cToPrintF = 6;
                    ret.cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_6RETRO":
                    ret.cToPrintF = 6;
                    ret.cToPrintR = 1;
                    break;


                //4 colori offset fronte e retro
                case "STAMPAOFF_FR_COL":
                    ret.cToPrintF = 4;
                    ret.cToPrintR = 4;
                    break;

                case "STAMPAOFF_FR_BN":
                    ret.cToPrintF = 1;
                    ret.cToPrintR = 1;
                    break;

                case "STAMPAOFF_FRONTE_COL":
                    ret.cToPrintF = 4;
                    ret.cToPrintR = 0;
                    break;

                case "STAMPAOFF_FRONTE_BN":
                    ret.cToPrintF = 1;
                    ret.cToPrintR = 0;
                    break;

                case "STAMPAOFFeDIGITALE_FR_COL":
                    ret.cToPrintF = 4;
                    ret.cToPrintR = 4;
                    break;

                case "STAMPAOFFeDIGITALE_FR_BN":
                    ret.cToPrintF = 1;
                    ret.cToPrintR = 1;
                    break;

                case "STAMPAOFFeDIGITALE_FRONTE_COL":
                    ret.cToPrintF = 4;
                    ret.cToPrintR = 0;
                    break;

                case "STAMPAOFFeDIGITALE_FRONTE_BN":
                    ret.cToPrintF = 1;
                    ret.cToPrintR = 0;
                    break;

                default:
                    ret.cToPrintF = 0;
                    ret.cToPrintR = 0;
                    //  throw new Exception();
                    break;
            }


            if (codOptionTypeOfTask.Contains("VERNICE"))
            {
                ret.cToPrintFNoImplant = 1;
                ret.cToPrintTNoImplant = 1;
            }

            ret.cToPrintT = ret.cToPrintF + ret.cToPrintR;
            ret.cToPrintTNoImplant = ret.cToPrintFNoImplant + ret.cToPrintRNoImplant;

            return ret;

        }



    }
}
