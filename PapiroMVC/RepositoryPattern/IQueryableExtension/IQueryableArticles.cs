using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{

    public static class Extensions
    {
        /// <summary>
        /// restituisce l'articolo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static IQueryable<Article> GetArticlesByProductPartPrintableArticle(this IQueryable<Article> query,
                                                 ProductPartsPrintableArticle a)
        {
            return query.OfType<Printable>().Where(x => x.NameOfMaterial == a.NameOfMaterial &&
                x.TypeOfMaterial == a.TypeOfMaterial &&
                x.Weight == a.Weight &&
                x.Color == a.Color);
        }
    }

}
