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
        List<ServiceConfig> services = new List<ServiceConfig>();
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
           
            hubConnection.On<ServiceConfig>("AddServerConfig", x =>
            {
                _logger.LogInformation("Adding {service}", x.ServiceName);
                services.Add(x);
            });
            hubConnection.On<ServiceConfig>("RemoveService", x =>
            {
                _logger.LogInformation("Removing {service}",x.ServiceName);
                services.RemoveAt(services.FindIndex(0,s => s.ServiceName == x.ServiceName));
            });
            await hubConnection.StartAsync();
            await hubConnection.SendAsync("RegisterServer", Environment.MachineName);
            int interval = await service.GetInterval();
            services = await service.GetServices(Environment.MachineName);
            if(interval == 0) interval = 1;
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking {count} service(s)", services.Count);
                Parallel.ForEach(services, async item =>
                {
                    try
                    {
                        _logger.LogInformation("Checking service {name}", item.ServiceName);
                        using var ps = PowerShell.Create();

                        var results = ps.AddScript($"get-service {item.ServiceName} ").Invoke();
                        foreach (var result in results)
                        {
                            var isRunning = result.Properties["Status"].Value.ToString() == "Running";
                            await service.UpdateStatus(item.Id, isRunning ? "Up" : "Down");
                            _logger.LogInformation("Service {name} is {status}", item.ServiceName, isRunning ? "Up" : "Down");
                        }
                    }
                    catch(Exception ex) { _logger.LogError("Error",ex); }
                });
                await Task.Delay(TimeSpan.FromMinutes(interval), stoppingToken);

            }
        }
    }
}
