using Leopard.Core.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Leopard.Shop.Base.Silo
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateDefaultBuilder(args)
                .Build()
                .Start()
                .Wait();
        }

        public static IOrleansHostBuilder CreateDefaultBuilder(string[] args) => OrleansHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}
