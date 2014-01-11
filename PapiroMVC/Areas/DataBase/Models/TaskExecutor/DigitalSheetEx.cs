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
    [MetadataType(typeof(DigitalSheet_MetaData))]
    public partial class DigitalSheet : Digital
    {

        public override double Starts(string codOptionTypeOfTask)
        {

            double starts = 0;

            switch (codOptionTypeOfTask)
            {
                //4 colori offset fronte e retro
                case "STAMPAOFF_FR_COL":
                    if (ColorSide2 ?? false)
                    {
                        starts = 1;
                    }
                    else
                    {
                        starts = 2;
                    }
                    break;

                case "STAMPAOFF_FR_BN":
                    if (BWSide2 ?? false)
                    {
                        starts = 1;
                    }
                    else
                    {
                        starts = 2;
                    }

                    break;

                case "STAMPAOFF_FRONTE_COL":
                    starts = 1;
                    break;

                case "STAMPAOFF_FRONTE_BN":
                    starts = 1;
                    break;

                case "STAMPAOFFeDIGITALE_FR_COL":
                    if (ColorSide2 ?? false)
                    {
                        starts = 1;
                    }
                    else
                    {
                        starts = 2;
                    }
                    break;

                case "STAMPAOFFeDIGITALE_FR_BN":
                    if (BWSide2 ?? false)
                    {
                        starts = 1;
                    }
                    else
                    {
                        starts = 2;
                    }

                    break;

                case "STAMPAOFFeDIGITALE_FRONTE_COL":
                    starts = 1;
                    break;

                case "STAMPAOFFeDIGITALE_FRONTE_BN":
                    starts = 1;
                    break;

                case "STAMPADIGITALE_FR_COL":
                    if (ColorSide2 ?? false)
                    {
                        starts = 1;
                    }
                    else
                    {
                        starts = 2;
                    }
                    break;

                case "STAMPADIGITALE_FR_BN":
                    if (BWSide2 ?? false)
                    {
                        starts = 1;
                    }
                    else
                    {
                        starts = 2;
                    }

                    break;

                case "STAMPADIGITALE_FRONTE_COL":
                    starts = 1;
                    break;

                case "STAMPADIGITALE_FRONTE_BN":
                    starts = 1;
                    break;

                default:
                    throw new Exception();
                    break;
            }

            return starts;
        }



        public DigitalSheet()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.DigitalSheet;
        }

        #region Added Properties

        #endregion

    }
}
