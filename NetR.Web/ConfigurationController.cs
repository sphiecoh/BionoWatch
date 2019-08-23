using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetR.Web.Infrastructure;
using NetR.Web.Infrastructure.Entities;
using NetR.Web.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetR.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : Controller
    {
        private readonly BionoContext bionoContext;
        public ConfigurationController(BionoContext bionoContext)
        {
            this.bionoContext = bionoContext;
        }
        // GET: api/<controller>
        [HttpGet]
        public object Get()
        {
            return new {
                Services = bionoContext.ServiceConfiguration.Select(x => new ServiceModel {Id = x.Id, Status = x.Status.ToString(), ServiceName = x.ServiceName, ServerName = x.ServerName, Enabled = x.Enabled, ResponsibleServer = x.ResponsibleServer }),
                Interval = bionoContext.Configuration.FirstOrDefault(c => c.Key == "PollingInterval").Value,
                EmailReciptients = bionoContext.Configuration.Where(e => e.Key =="NotificationEmail").Select(p => new { p.Id , Email = p.Value })
            };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IEnumerable<object> Get(int id)
        {
            return bionoContext.Configuration;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task Post(ServiceModel model)
        {
           await bionoContext.ServiceConfiguration.AddAsync(new ServiceConfiguration
           {
               Enabled = model.Enabled,
               ServerName = model.ServerName,
               ResponsibleServer = model.ResponsibleServer,
               ServiceName = model.ServiceName,
               Status = ServiceStatus.Unchecked
           });
           await bionoContext.SaveChangesAsync();
        }
        [HttpPost]
        [Route("interval")]
        public async Task PostInterval(string pollingInterval)
        {
            await bionoContext.Configuration.AddAsync(new Configuration
            {
                Key = "PollingInterval",
                Value = pollingInterval
            });
            await bionoContext.SaveChangesAsync();
        }
        [HttpPost]
        [Route("notification")]
        public async Task PostNotification(string email)
        {
            await bionoContext.Configuration.AddAsync(new Configuration
            {
                Key = "NotificationEmail",
                Value = email
            });
            await bionoContext.SaveChangesAsync();
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
