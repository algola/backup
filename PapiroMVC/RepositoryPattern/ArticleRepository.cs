using System;
using System.Linq;
using PapiroMVC.Models;

namespace Services
{
    public class ArticleRepository : GenericRepository<dbEntities, Article>, IArticleRepository
    {

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
            var query = Context.articles.Include("articlecosts").FirstOrDefault(x => x.CodArticle == codArticle);
            return query;
        }

        public override void SetDbName(string name)
        {
 	         base.SetDbName(name);
        }
    }
}