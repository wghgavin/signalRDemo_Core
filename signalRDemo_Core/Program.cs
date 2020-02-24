using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace signalRDemo_Core
{
    class Program
    {
        static HubConnection connection;
        static void Main(string[] args)
        {
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44391/ChatHub")
                .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            Connect();
            Console.ReadKey();
            Send();
            Console.ReadKey();
        }
        private static async void Connect()
        {
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                    var newMessage = $"{user}: {message}";
                    Console.WriteLine(newMessage);
            });
            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connection started");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static async void Send()
        {
            try
            {
                await connection.InvokeAsync("SendMessage",
                    "gavin", "111");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
