using System;
using System.Collections.Generic;
using System.Web.Http;
using FairlieAuthentic.Models;
using Mindscape.LightSpeed;


namespace FairlieAuthentic.Controllers
{
    public class CustomerController : ApiController
    {
        protected LightSpeedContext<FairlieAuthenticUnitOfWork> LightSpeedContext
        {
            get { return WebApiApplication.FaLightSpeedContext; }
        }
        // GET api/customer
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/customer/5
        public Customer Get(int id)
        {
            var cust = new Customer {Email = "bob@bob.r.us", Name = "bob"};
            return cust;
        }

        // POST api/customer
        public void Post([FromBody]Customer customer)
        {
            using (var uow = LightSpeedContext.CreateUnitOfWork())
            {
                Customer cust = new Customer
                    {
                        Name = customer.Name, 
                        Email = customer.Email
                    };
                uow.Add(cust);
                uow.SaveChanges();
            }
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
