using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    #region Gestione copie e valori degli oggetti connessi ad un altro oggetto

    public interface IDeleteRelated
    {
        //Rende null gli oggetti connessi ad un altro oggetto
        void ChildsNull();
    }

    #endregion
}