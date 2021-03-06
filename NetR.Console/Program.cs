﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
namespace NetR.Console.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            HubConnection hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5000/hub").Build();
            await hubConnection.StartAsync();
            await hubConnection.SendAsync("RegisterServer",Dns.GetHostName());
            hubConnection.On<string>("AddServerConfig", x => {
                System.Console.WriteLine(x);
            });
           // var client = new System.Net.HttpClient { B}
            System.Console.Read();
        }
    }
}
