using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{

    public partial class BindingTsk : PrePostPress, IDataErrorInfo, ICloneable, IDeleteRelated
    {


        public BindingTsk()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.Binding;
        }

        #region Proprietà aggiuntive

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
               };

        public override string this[string proprieta]
        {
            get
            {
                string result = base[proprieta];
                return result;
            }
        }

        //Check validation of entity
        public override bool IsValid
        {
            get
            {
                bool ret = true;
                foreach (string prop in proprietaDaValidare)
                {
                    if (this[prop] != null)
                        ret = false;
                }
                return ret && base.IsValid;

            }
        }

        #endregion

        #region Handle copy for modify

        public override void Copy(TaskExecutor to)
        {
            base.Copy(to);
        }

        #endregion
    }


}
