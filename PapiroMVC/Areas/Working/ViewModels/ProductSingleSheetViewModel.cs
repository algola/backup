using PapiroMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Services;

namespace PapiroMVC.Models
{

    public class ProductViewModel
    {
        //generic product
        protected Product product;
        protected List<ProductPart> productParts;
        protected List<ProductPartsPrintableArticle> productPartArticles;

        //tasks in page 
        protected List<TypeOfTask> pageTasks;
        protected List<ProductTask> productTasks;

        public virtual void InitPageTask(IQueryable<TypeOfTask> taskList)
        { 
        
        }

        public virtual void InitProductTask(IQueryable<TypeOfTask> taskList)
        { 
        
        }


        //initialize only posiible part task like... plastificatura etc...
        public List<ProductPartTask> GetInitalizedPartTask(IQueryable<TypeOfTask> taskList)
        {
            var tsksInPart = new List<ProductPartTask>();

            ProductPartTask pt;

            String[] codTypeOfTasks = { "STAMPA", "PLASTIFICATURA" };

            foreach (var item in codTypeOfTasks)
            {
                pt = new ProductPartTask();
                //default selection
                pt.OptionTypeOfTask = taskList.FirstOrDefault(x => x.CodTypeOfTask == item).OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item + "-NO");
                pt.CodOptionTypeOfTask = pt.OptionTypeOfTask.CodOptionTypeOfTask;
                pt.Hidden = true;
                tsksInPart.Add(pt);
            }

            return tsksInPart;

        }


    }

    public class ProductSingleSheetViewModel : ProductViewModel
    {

        //init for this kind of product
        public override void InitPageTask(IQueryable<TypeOfTask> taskList)
        {            
            this.TsksInPage = taskList.ToList();
        }


        public override void InitProductTask(IQueryable<TypeOfTask> taskList)
        {
            var tsksInProduct = new List<ProductTask>();

            ProductTask pt;
            
            foreach (var item in taskList)
            {
                pt = new ProductTask();
                //default selection
                pt.OptionTypeOfTask = item.OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item.CodTypeOfTask + "-NO");
                pt.CodOptionTypeOfTask = pt.OptionTypeOfTask.CodOptionTypeOfTask;
                pt.Hidden = true; 
                tsksInProduct.Add(pt);
            }

            this.ProductTasks = tsksInProduct;
        
        }

        public ProductSingleSheetViewModel()
        {
            product = new ProductSingleSheet();

            //initialize Parts
            productParts = new List<ProductPart>();
            pageTasks = new List<TypeOfTask>();
            productTasks = new List<ProductTask>();        
        }

        public List<ProductPart> ProductParts
        {
            get 
            {
                return productParts;
            }
            set 
            {
                productParts = value;
            }
        }

        public Product Product
        {
            get
            {
                return product;
            }

            set 
            {
                product = value;
            }
        }

        public List<TypeOfTask> TsksInPage
        {
            get
            {
                return pageTasks;
            }
            set
            {
                pageTasks = value;
            }
        }

        public List<ProductTask> ProductTasks
        {
            get
            {
                return productTasks.OrderBy(x=>x.IndexOf).ToList();
            }
            set
            {
                productTasks = value;
            }
        }

    }

}