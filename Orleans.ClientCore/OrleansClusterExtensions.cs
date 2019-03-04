using Leopard.Core.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans.ClientCore;
using Orleans.ClientCore.Config;
using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans
{
    public static class OrleansClusterExtensions
    {
        public static IServiceCollection AddOrleansClient(this IServiceCollection serviceCollection, IConfigurationSection clusterConfigOptions)
        {
            serviceCollection.Configure<OrleansClusterConfigOptions>(clusterConfigOptions);
            serviceCollection.AddSingleton<OrleansClusterConfigHelper, OrleansClusterConfigHelper>();
            serviceCollection.AddSingleton<OrleansClusterProxy, OrleansClusterProxy>();
            return serviceCollection;
        }
    }
}
