using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    //public abstract class ProductCreator
    //{
    //    public virtual abstract Product Create();
    //}

    ///// <summary>
    ///// Concrete Creator SingleSheet
    ///// </summary>
    //public class ProductSingleSheetCreator : ProductCreator
    //{
    //    public override Product Create()
    //    {
    //        return new ProductSingleSheet();
    //    }
    //}

    ///// <summary>
    ///// Concrete Creator ProductBookSheet
    ///// </summary>
    //public class ProductBookSheetCreator : ProductCreator
    //{
    //    public override Product Create()
    //    {
    //        return new ProductBookSheet();
    //    }
    //}

    ///// <summary>
    ///// Concrete Creator ProductBookSheet
    ///// </summary>
    //public class ProductRigidCreator : ProductCreator
    //{
    //    public override Product Create()
    //    {
    //        return new ProductRigid();
    //    }
    //}

    public static class Creator
    {
        public static Product InitProduct(string id, IProductTaskNameRepository prodTskNameRepository, IFormatsNameRepository formatsRepository, ITypeOfTaskRepository typeOfTaskRepository)
        {
            Product product;
            product = new ProductSingleSheet();

            if (id == "Buste" ||
                id == "Volantini" ||
                id == "Pieghevoli" ||
                id == "CartaIntestata" ||
                id == "Locandine" ||
                id == "FogliMacchina")
            {
                product = new ProductSingleSheet();
            }

            if (
                id == "BigliettiVisita" ||
                id == "EtichetteCartellini" ||
                id == "CartolineInviti" ||
                id == "CartolinePostali" ||
                id == "AltriFormati")
            {
                product = new ProductSingleSheet();
                product.ShowDCut = true;
                product.DCut = 0.5;
            }

            if (id == "PuntoMetallico" ||
                id == "SpiraleMetallica" ||
                id == "BrossuraFresata" ||
                id == "BrossuraCucitaFilo" ||
                id == "RivistePostalizzazione" ||
                id == "SchedeNonRilegate")
            {
                product = new ProductBookSheet();
            }


            if (
                id == "Fotoquadri" ||
                id == "SuppRigidi" ||
                id == "Poster")
            {
                product = new ProductRigid();
                product.ShowDCut = true;
                product.DCut = 2;
            }

            if (
                id == "PVC" ||
                id == "Manifesti" ||
                id == "Striscioni")
            {
                product = new ProductRigid();
            }

            product.CodMenuProduct = id;
            product.ProductTaskName = prodTskNameRepository.GetAllById(id);
            product.FormatsName = formatsRepository.GetAllById(id);

            product.SystemTaskList = typeOfTaskRepository.GetAll();
            product.InitProduct();

            return product;

        }

    }

}