using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.UI.WebControls;


namespace FairlieAuthentic.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            List<string> cert2 = new List<string>();
            foreach (StoreLocation storeLocation in (StoreLocation[]) Enum.GetValues(typeof (StoreLocation)))
            {
                foreach (StoreName storeName in (StoreName[]) Enum.GetValues(typeof (StoreName)))
                {
                    X509Store store = new X509Store(storeName, storeLocation);
                    try
                    {
                        store.Open(OpenFlags.OpenExistingOnly);
                        cert2.Add(string.Format("Yes    {0,4}  {1}, {2}", store.Certificates.Count, store.Name, store.Location));
                        foreach (X509Certificate2 cert in store.Certificates)
                        {
                            cert2.Add(string.Format("          {0}, {1}, {2}", cert.FriendlyName, cert.Issuer, cert.Thumbprint));
                        }
                    }
                    catch (CryptographicException)
                    {
                        cert2.Add(string.Format("No           {0}, {1}", store.Name, store.Location));
                    }
                }
            }
            return cert2.ToArray();
        }

        // GET api/values/5
        public string Get(int id)
        {
            if (id == 1)
            { 
                return User.Identity.Name;
            }
            else if (id == 2)
            {
                string certs = "";
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                try
                {
                    store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
                    foreach (X509Certificate2 cert in store.Certificates)
                    {
                        certs += cert.FriendlyName + " - ";
                        certs += cert.Issuer + " - ";
                        certs += cert.Subject + " - ";
                        //// Expiration
                        //expirationText.Text = cert.NotAfter.ToString("d");
                    }
                }
                finally
                {
                    store.Close();
                }
                return "CERTS: " + certs;
            }
            

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