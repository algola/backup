using PapiroMVC.Models;
using PapiroMVC.Models.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.ServiceLayer
{
    public static class Projection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public  static void MakeProjection(ProductRigid from, ProductRigidApi to)
        {
            
            var part = from.ProductParts.FirstOrDefault();
            var material = part.ProductPartPrintableArticles.FirstOrDefault();

            to.Format = from.Format;
            to.TypeOfMaterial = material.TypeOfMaterial;
            to.NameOfMaterial = material.NameOfMaterial;
            to.Color = material.Color;
            to.Weight = material.Weight;

        }

        public static void ResolveProjection(ProductRigidApi from,  ProductRigid to)
        {

            var part = to.ProductParts.FirstOrDefault();
            var material = part.ProductPartPrintableArticles.FirstOrDefault();

            to.Format = from.Format;
            material.TypeOfMaterial = from.TypeOfMaterial;
            material.NameOfMaterial = from.NameOfMaterial;
            material.Color = from.Color;
            material.Weight = from.Weight;

        }

    }
}