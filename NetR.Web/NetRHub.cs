﻿using Microsoft.AspNetCore.SignalR;
using NetR.Web.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetR.Web
{
    public class NetRHub : Hub<INetRHub>
    {   
        public void RegisterServer(string server)
        {
            this.Groups.AddToGroupAsync(this.Context.ConnectionId,server);
        }
        
    }
    public interface INetRHub
    {
       Task AddServerConfig(ServiceModel service);
    }
}
