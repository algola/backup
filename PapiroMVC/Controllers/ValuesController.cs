using PapiroMVC.Models;
using PapiroMVC.ServiceLayer;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace PapiroMVC.Controllers
{
    /// <summary>
    /// help a
    /// </summary>
    [EnableCors("*", "*", "*")]
    public class ValuesController : ApiController        
    {
        /// <summary>
        /// get empty product initialized by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("api/empty/{id}")]
        [HttpGet]
        public HttpResponseMessage GetEmpty(string id)
        {
            try
            {
                id = "SuppRigidi";
                PapiroService papiro = new PapiroService();
                //work with product
                Product prod = papiro.InitProduct(id, new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());
                return Request.CreateResponse<Product>(HttpStatusCode.OK, prod);                
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        [Route("api/empty2/{id}")]
        [HttpGet]
        public Product GetEmpty2(string id)
        {
            try
            {
                id = "SuppRigidi";
                PapiroService papiro = new PapiroService();
                //work with product
                Product prod = papiro.InitProduct(id, new ProductTaskNameRepository(), new FormatsNameRepository(), new TypeOfTaskRepository());
                return prod;
            }
            catch (Exception)
            {
                return null;
            }
        }
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
