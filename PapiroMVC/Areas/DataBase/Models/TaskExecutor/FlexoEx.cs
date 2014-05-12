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
    [MetadataType(typeof(Flexo_MetaData))]
    public partial class Flexo : Litho
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Flexo()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.Flexo;
        }

        public override CostDetail.QuantityType TypeOfImplantQuantity
        {
            get
            {
                CostDetail.QuantityType ret = CostDetail.QuantityType.MqSheetTypeOfQuantity;
                return ret;
            }
        }


        public override void GetColorFR(string codOptionTypeOfTask, out double cToPrintF, out double cToPrintR, out double cToPrintT)
        {
            switch (codOptionTypeOfTask)
            {
                //4 colori offset fronte e retro
                case "STAMPAETICHROTOLO_1":
                    cToPrintF = 1;
                    cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_2":
                    cToPrintF = 2;
                    cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_3":
                    cToPrintF = 3;
                    cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_4":
                    cToPrintF = 4;
                    cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_5":
                    cToPrintF = 5;
                    cToPrintR = 0;
                    break;

                case "STAMPAETICHROTOLO_6":
                    cToPrintF = 6;
                    cToPrintR = 0;
                    break;

                default:
                    cToPrintF = 0;
                    cToPrintR = 0;
                    break;
            }

            cToPrintT = cToPrintF + cToPrintR;

        }

        /// <summary>
        /// get printing color (task vs machine)
        /// </summary>
        /// <param name="codOptionTypeOfTask"></param>
        /// <returns></returns>
        public override double Starts(string codOptionTypeOfTask)
        {
            double cToPrintF = 0;
            double cToPrintR = 0;
            double cToPrintT = 0;

            GetColorFR(codOptionTypeOfTask, out cToPrintF, out cToPrintR, out cToPrintT);

            //Starts is used with printerFormat to have
            return Math.Ceiling(cToPrintT / this.PrintingUnit ?? 1);
        }


        //get number of impants by type of task!! ex: 2 colors --> 2 implants
        public override double Implants(string codOptionTypeOfTask)
        {
            double cToPrintF = 0;
            double cToPrintR = 0;
            double cToPrintT = 0;

            GetColorFR(codOptionTypeOfTask, out cToPrintF, out cToPrintR, out cToPrintT);

            //Starts is used with printerFormat to have
            return Math.Ceiling(cToPrintT);
        }

        public int CurrentZ
        { get; set; }

        /// <summary>
        /// Check if Cylinder 0 are in list
        /// so view has row to insert a new cylinder Z
        /// </summary>
        public void CheckZeroCylinder()
        {
            var x = this.TaskExecutorCylinders.FirstOrDefault(y => y.Z == 0);
            if (x == null)
            {
                this.TaskExecutorCylinders.Add(new TaskExecutorCylinder { Z = 999, Quantity = 0 });
            }
        }

        #region Added Properties

        #endregion

    }
}
