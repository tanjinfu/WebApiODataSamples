using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductsController : ApiController
    {
        [MyEnableQuery]
        public IHttpActionResult Get()
        {
            IList<Product> products=new List<Product>();
            products.Add(new Product() { Id = 1, Name = "Name1",Category=new Category(){Id=1,Name="Category1" }});
            products.Add(new Product() { Id = 2, Name = "Name2", Category = new Category() { Id = 2, Name = "Category2" } });


            return Ok(products.AsQueryable<Product>());
        }
    }
}
