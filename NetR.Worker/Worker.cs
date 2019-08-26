using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetR.Worker.Models;
using Refit;

namespace NetR.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public IConfiguration Configuration { get; }
        IEnumerable<ServiceConfig> services = new List<ServiceConfig>();
        private IService service;
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            this.Configuration = configuration;
            _logger = logger;
            service = RestService.For<IService>(Configuration["ApiBase"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            HubConnection hubConnection = new HubConnectionBuilder().WithUrl($"{Configuration["ApiBase"]}/hub").Build();
            await hubConnection.StartAsync();
            await hubConnection.SendAsync("RegisterServer", Environment.MachineName);
            hubConnection.On<ServiceConfig>("AddServerConfig", x =>
            {
                services.Append(x);
            });
            int interval = await service.GetInterval();
            services = await service.GetServices(Environment.MachineName);
            if(interval == 0) interval = 1;
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromMinutes(interval), stoppingToken);
                foreach (var item in services)
                {
                    using (var ps = PowerShell.Create())
                    {
                        var results = ps.AddScript("command").Invoke();
                        foreach (var result in results)
                        {
                            //Debug.Write(result.ToString());
                        }
                    }
                }

            }
        }
    }
}
