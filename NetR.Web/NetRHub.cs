using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web
{
    public class NetRHub : Hub
    {
        public void Publish(string message)
        {
            Clients.Others.SendAsync("Recieve", message);
        }
    }
}
