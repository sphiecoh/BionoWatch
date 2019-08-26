using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IHubContext<NetRHub, INetRHub> hubContext;

        public ConfigurationController(BionoContext bionoContext,IHubContext<NetRHub,INetRHub> hubContext)
        {
            this.bionoContext = bionoContext;
            this.hubContext = hubContext;
        }
        // GET: api/<controller>
        [HttpGet]
        public object Get()
        {
            return new {
                Services = bionoContext.ServiceConfiguration.Select(x => new ServiceModel {Id = x.Id, Status = x.Status.ToString(), ServiceName = x.ServiceName, ServerName = x.ServerName, Enabled = x.Enabled, ResponsibleServer = x.ResponsibleServer }),
                Interval = bionoContext.Configuration.FirstOrDefault(c => c.Key == "PollingInterval")?.Value,
                EmailReciptients = bionoContext.Configuration.Where(e => e.Key =="NotificationEmail").Select(p => new { p.Id , Email = p.Value })
            };
        }
        [HttpGet]
        [Route("interval")]
        public int GetInterval()
        {
             int.TryParse(bionoContext.Configuration.FirstOrDefault(c => c.Key == "PollingInterval")?.Value,out int interval);
            return interval;
        }

        // GET api/<controller>/5
        [HttpGet("{serverName}/services")]
        public IEnumerable<ServiceConfiguration> GetServices(string serverName)
        {
            return bionoContext.ServiceConfiguration.Where(s => s.ResponsibleServer == serverName);
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
           await hubContext.Clients.Group(model.ResponsibleServer).AddServerConfig(model);
           
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
        [HttpPost("{id}/status/{status}")]
        public async Task UpdateStatus(int id, [FromRoute]string status)
        {
            var service = await bionoContext.ServiceConfiguration.FirstAsync(s => s.Id == id);
            service.Status = (ServiceStatus)Enum.Parse(typeof(ServiceStatus), status);
            await bionoContext.SaveChangesAsync();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
