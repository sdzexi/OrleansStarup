using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Runtime.MembershipService;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Core.Hosting
{
    public static class AdoNetClusteringExtensions
    {
        public static ISiloHostBuilder UseAdoNetClustering(this ISiloHostBuilder builder,Func<HostBuilderContext,IConfiguration> hostBuilderContext)
        {
            return builder.ConfigureServices((context, collection) => {

                builder.Configure<AdoNetClusteringSiloOptions>(collection, hostBuilderContext(context));

                collection.AddSingleton<IMembershipTable, AdoNetClusteringTable>();
                collection.AddSingleton<IConfigurationValidator, AdoNetClusteringSiloOptionsValidator>();
            });
        }
    }
}
