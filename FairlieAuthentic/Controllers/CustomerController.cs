using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FairlieAuthentic.Models;

namespace FairlieAuthentic.Controllers
{
    public class CustomerController : ApiController
    {
        // GET api/customer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/customer/5
        public Customer Get(int id)
        {
            var cust = new Customer {Email = "bob@bob.r.us", Username = "bob"};
            return cust;
        }

        // POST api/customer
        public void Post([FromBody]string value)
        {
        }

        // PUT api/customer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/customer/5
        public void Delete(int id)
        {
        }
    }
}
