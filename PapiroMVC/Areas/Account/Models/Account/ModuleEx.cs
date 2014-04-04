
namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(Module_MetaData))]
    public partial class Module
    {

        public enum StatusType : int
        {
            Valuating = 0,
            Activeted = 1,
            NotActivated = 2,
            NotActivetedValuable = 3,

        }
        public override string ToString()
        {
            return "";
        }

        public void CheckStatus()
        {
            int result = DateTime.Compare(ExpirationDate??DateTime.Today, DateTime.Today);
            if (result < 0)
            {
                switch (Status)
                {
                    case 0:
                        Status = 2;
                        ExpirationDate = DateTime.Today.AddMonths(1);
                        ActivationDate = DateTime.Today;
                        break;
                    case 1:
                        Status = 2;
                        ExpirationDate = DateTime.Today.AddMonths(1);
                        ActivationDate = DateTime.Today;
                        break;
                    case 2:
                        Status = 3;
                        ActivationDate = DateTime.Today;
                        break;
                    default:
                        break;
                }
            }

        }

        public void ChangeAcquired(int months)
        {
            int diff;
            switch (Status)
            {
                case 0:
                    Status = 1;
                    diff = (ExpirationDate ?? DateTime.Today).Subtract(DateTime.Today).Days;
                    ExpirationDate = DateTime.Today.AddMonths(months).AddDays(diff);
                    ActivationDate = DateTime.Today;
                    break;
                case 1:
                    diff = (ExpirationDate ?? DateTime.Today).Subtract(DateTime.Today).Days;
                    ExpirationDate = DateTime.Today.AddMonths(months).AddDays(diff);
                    ActivationDate = DateTime.Today;
                    break;
                case 2:
                    Status = 1;
                    ExpirationDate = DateTime.Today.AddMonths(months);
                    ActivationDate = DateTime.Today;
                    break;
                case 3:
                    Status = 1;
                    ExpirationDate = DateTime.Today.AddMonths(months);
                    ActivationDate = DateTime.Today;
                    break;
                default:
                    break;
            }
        }

        public void ChangeInValuating()
        {
            Status = 0;
            ExpirationDate = DateTime.Today.AddMonths(1);
            ActivationDate = DateTime.Today;
        }


        public bool IsValid
        {
            get
            {
                CheckStatus();
                return (Status == 0 || Status == 1);
            }
        }

    }
}
