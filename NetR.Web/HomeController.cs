using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NetR.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetR.Web
{
    public class HomeController : Controller
    {
        private readonly IHubContext<NetRHub> hub;
        public HomeController(IHubContext<NetRHub> hub)
        {
            this.hub = hub;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody]ServiceModel service)
        {
            await hub.Clients.All.SendAsync("Recieve", service);
            return Ok();
        }
    }
}
