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
                EmailReciptients = bionoContext.EmailNotification.ToArray()
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
            return bionoContext.ServiceConfiguration.Where(s => s.ResponsibleServer.Equals(serverName,StringComparison.InvariantCultureIgnoreCase));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task Post(ServiceModel model)
        {
            var service = new ServiceConfiguration
            {
                Enabled = model.Enabled,
                ServerName = model.ServerName,
                ResponsibleServer = model.ResponsibleServer,
                ServiceName = model.ServiceName,
                Status = ServiceStatus.Unchecked
            };
           await bionoContext.ServiceConfiguration.AddAsync(service);
           await bionoContext.SaveChangesAsync();
            model.Id = service.Id;
           await hubContext.Clients.Group(model.ResponsibleServer.ToLower()).AddServerConfig(model);
           
        }
        [HttpPut]
        public async Task<IActionResult> Update(ServiceModel model)
        {
            var service = await bionoContext.ServiceConfiguration.FindAsync(model.Id);
            if (service == null) return NotFound();
            service.Enabled = model.Enabled;
            await bionoContext.SaveChangesAsync();
            if(!service.Enabled) await hubContext.Clients.Group(service.ResponsibleServer).RemoveService(service);
            return Ok();

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
        public async Task<object> PostNotification(string email)
        {
            var conf = new Infrastructure.Entities.EmailNotification
            {
               Email = email
            };
            await bionoContext.EmailNotification.AddAsync(conf);
            await bionoContext.SaveChangesAsync();
            return new { id = conf.Id, email };
        }

        // PUT api/<controller>/5
        [HttpPost("{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromRoute]string status)
        {
            
            var service = await bionoContext.ServiceConfiguration.FirstAsync(s => s.Id == id);
            if (service == null) return NotFound();
            service.Status = (ServiceStatus)Enum.Parse(typeof(ServiceStatus), status);
            await bionoContext.SaveChangesAsync();
            await hubContext.Clients.All.Refresh();
            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var service = await bionoContext.ServiceConfiguration.FirstAsync(s => s.Id == id);
            if (service == null) return NotFound();
            bionoContext.ServiceConfiguration.Remove(service);
            await bionoContext.SaveChangesAsync();
            await hubContext.Clients.Group(service.ResponsibleServer).RemoveService(service);
            return Ok();
        }
    }
}
