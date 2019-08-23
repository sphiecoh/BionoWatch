using System;
using Microsoft.AspNetCore.SignalR.Client;
namespace NetR.Console.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            HubConnection hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5000/hub").Build();
            hubConnection.StartAsync().Wait();
            hubConnection.On<string>("Recieve", x => {
                System.Console.WriteLine(x);
            });
            System.Console.Read();
        }
    }
}
