using PapiroMVC.ServiceLayer;
using PapiroMVC.Models.WebApi;
using PapiroMVC.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ModelBinding;

namespace PapiroMVC.Controllers
{

    /// <summary>
    /// help a
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class ProductApiController : ApiController
    {
        IArticleRepository articleRepository;
        public ProductApiController(IArticleRepository _articleRepository)
        {
            articleRepository = _articleRepository;
        }
        
        /// <summary>
        /// get empty product initialized by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/rigid/empty/{id}")]
        [HttpGet]
        public HttpResponseMessage GetEmptyRigid(string id)
        {
            try
            {
                PapiroService papiro = new PapiroService();
                //Initialize new product
                PapiroMVC.Models.ProductRigid prodIntero = (ProductRigid)papiro.InitProduct(id, new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

                var prod = new ProductRigidApi();
                prod.Id = id;
                
                Projection.MakeProjection(prodIntero, prod);

                return Request.CreateResponse<ProductRigidApi>(HttpStatusCode.OK, prod);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// get back product with its prices, discounted by date
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        [Route("api/rigid/productprice")]
        [HttpPost]
        public HttpResponseMessage GetPriceRigid([FromBody]ProductRigidApi prod)
        {
            try
            {
                PapiroService papiro = new PapiroService();
                //Initialize new product
                PapiroMVC.Models.ProductRigid prodIntero = (ProductRigid)papiro.InitProduct(prod.Id, new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());

                Projection.ResolveProjection(prod, prodIntero);

                //generazione dei prezzi a caso
                prod.Prices.Clear();
                prod.Prices.Add(new Price { Date = DateTime.Today.AddDays(2), UnitPrice = "0.087" });
                prod.Prices.Add(new Price { Date = DateTime.Today.AddDays(4), UnitPrice = "0.059" });
                prod.Prices.Add(new Price { Date = DateTime.Today.AddDays(9), UnitPrice = "0.024" });

                return Request.CreateResponse<ProductRigidApi>(HttpStatusCode.OK, prod);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// return Rigid Material
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        [Route("api/rigid/materials")]
        [HttpGet]
        public HttpResponseMessage GetMaterialRigid()
        {
            try
            {
                PapiroService papiro = new PapiroService();
                //Initialize new product
                var x = papiro.GetRigidList(new ArticleRepository());

                return Request.CreateResponse<List<PrintableArticleApi>>(HttpStatusCode.OK, x);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }



    }
}
