using Microsoft.AspNetCore.Mvc;
using QIQO.Business.ViewModels.Api;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace QIQO.Business.Api.Controllers
{
    [Route("api/carts")]
    public class CartController : Controller
    {
        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            return Json(new List<CartViewModel>());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            return Json(new CartViewModel());
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]CartViewModel value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]CartViewModel value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}
