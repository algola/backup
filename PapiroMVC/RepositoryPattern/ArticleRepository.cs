using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;

namespace Services
{
    public class ArticleRepository : GenericRepository<dbEntities, Article>, IArticleRepository
    {
        public string GetNewCode(Article a, ICustomerSupplierRepository customerSupplierRepository, string supplierMaker, string supplyerBuy)
        {
            CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

            var filteredItems = customerSuppliers.Where(
                item => item.BusinessName.IndexOf(supplierMaker, StringComparison.InvariantCultureIgnoreCase) >= 0);

            if (filteredItems.Count() == 0) throw new Exception();

            a.CodSupplierMaker = filteredItems.Single().CodCustomerSupplier;

            customerSuppliers = customerSupplierRepository.GetAll().ToArray();

            var filteredItems2 = customerSuppliers.Where(
                item => item.BusinessName.IndexOf(supplyerBuy, StringComparison.InvariantCultureIgnoreCase) >= 0);

            if (filteredItems2.Count() == 0) throw new Exception();

            //if #suppliers < 1 then no supplier has selected correctly and then thow error
            a.CodSupplierBuy = filteredItems2.Single().CodCustomerSupplier;

            var codes = (from COD in this.GetAll() select COD.CodArticle).ToArray().OrderBy(x => x, new SemiNumericComparer());

            var csCode = codes.Last();

            if (csCode == null)
                csCode = "0";
            return AlphaCode.GetNextCode(csCode);

        }

        public override void Add(Article entity)
        {
            //cehck if name is just inserted
            var article = (from ART in this.GetAll() where ART.ArticleName == entity.ArticleName select ART);
            if (article.Count() > 0)
            {
                //this.Edit(entity);
            }
            else
                base.Add(entity);
        }

        public override IQueryable<Article> GetAll()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.articles.Include("articlecosts").Include("CustomerSupplierMaker").Include("CustomerSupplierBuy");
        }

        public override void Edit(Article entity)
        {
            foreach (var item in entity.ArticleCosts)
            {
                Context.Entry(item).State = System.Data.EntityState.Modified;
            }
            base.Edit(entity);
        }

        public Article GetSingle(string codArticle)
        {
            var query = Context.articles.Include("articlecosts").Include("CustomerSupplierMaker").Include("CustomerSupplierBuy").FirstOrDefault(x => x.CodArticle == codArticle);
            return query;
        }

        public override void SetDbName(string name)
        {
 	         base.SetDbName(name);
        }
    }
}