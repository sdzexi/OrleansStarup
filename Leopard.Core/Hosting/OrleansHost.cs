using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime.Startup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Leopard.Core.Hosting
{
    public static class OrleansHost
    {
        private static ISiloHostBuilder _SiloHostBuilder { get; set; }

        public static IOrleansHostBuilder CreateDefaultBuilder(string[] args)
        {

            _SiloHostBuilder = new SiloHostBuilder()
               .ConfigureAppConfiguration((hostBuilderContext, appConfiguration) =>
               {
                   IHostingEnvironment env = hostBuilderContext.HostingEnvironment;
                   appConfiguration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                   //appConfiguration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                   if (args?.Length > 0)
                   {
                       appConfiguration.AddCommandLine(args);
                   }
               })
               .Configure<ClusterOptions>(hostBuilderContext => {
                   return hostBuilderContext.Configuration.GetSection(nameof(ClusterOptions));
               })
               .UseAdoNetClustering(hostBuilderContext => {
                   return hostBuilderContext.Configuration.GetSection(nameof(AdoNetClusteringSiloOptions));
               })
               .ConfigureEndPointOptions(hostBuilderContext => {
                   return hostBuilderContext.Configuration.GetSection(nameof(SiloEndPointOptions));
               })
               .ConfigureLogging((hostBuilderContext, logging) =>
               {
                   logging.AddConfiguration(hostBuilderContext.Configuration.GetSection("Logging"));
                   logging.AddConsole();
               });
              

            return (IOrleansHostBuilder)new OrleansHostBuilder(_SiloHostBuilder);
        }
    }
}
