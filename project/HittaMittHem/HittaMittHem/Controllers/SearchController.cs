using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HittaMittHem.Domain.Webservices;
using HittaMittHem.Domain.Entities.Booli;

namespace HittaMittHem.Controllers
{
    public class SearchController : ApiController
    {
        // GET api/search
        public IEnumerable<Ad> Get()
        {
            return new BooliWebservice().Search("nacka");
        }

        // GET api/search/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/search
        public void Post([FromBody]string value)
        {
        }

        // PUT api/search/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/search/5
        public void Delete(int id)
        {
        }
    }
}
