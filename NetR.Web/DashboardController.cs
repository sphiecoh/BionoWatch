using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetR.Web.Infrastructure;
using NetR.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetR.Web
{
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly BionoContext bionoContext;

        public DashboardController(BionoContext bionoContext)
        {
            this.bionoContext = bionoContext;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<ServiceModel> Get()
        {
            return bionoContext.ServiceConfiguration.Select(x => new ServiceModel {Id = x.Id, Status = x.Status.ToString(), ServiceName = x.ServiceName, ServerName = x.ServerName, Enabled = x.Enabled, ResponsibleServer = x.ResponsibleServer });
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
