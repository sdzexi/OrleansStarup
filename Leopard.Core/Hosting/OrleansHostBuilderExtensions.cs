using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using AdoNetClusteringSiloOptions = Orleans.Configuration.AdoNetClusteringSiloOptions;
using System.Net;
using Leopard.Common;
using Leopard.Core.Startup;
using StartupLoader = Leopard.Core.Startup.StartupLoader;
using IStartup = Leopard.Core.Startup.IStartup;
using Orleans.Runtime.Configuration;
using System.Net.Sockets;

namespace Leopard.Core.Hosting
{

    public static class OrleansHostBuilderExtensions
    {
        public static IOrleansHostBuilder UseStartup<TStartup>(this IOrleansHostBuilder hostBuilder) where TStartup : class
        {
            Type startupType = typeof(TStartup);
            string name = startupType.GetTypeInfo().Assembly.GetName().Name;
            return hostBuilder.ConfigureServices((Action<IServiceCollection>)(services =>
            {
                if (typeof(IStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    ServiceCollectionServiceExtensions.AddSingleton(services, typeof(IStartup), startupType);
                else
                    ServiceCollectionServiceExtensions.AddSingleton(services, typeof(IStartup), (Func<IServiceProvider, object>)(sp =>
                    {
                        IHostingEnvironment requiredService = sp.GetRequiredService<IHostingEnvironment>();
                        return (object)new StartupHelper(StartupLoader.LoadMethods(sp, startupType));
                    }));
            }));
        }


        public static ISiloHostBuilder Configure<TOptions>(this ISiloHostBuilder builder, Func<HostBuilderContext,IConfiguration> hostBuilderContext) where TOptions : class
        {
            return builder.ConfigureServices((context, collection) => {
                collection.AddOptions<TOptions>().Bind(hostBuilderContext(context));
            });
        }

        public static ISiloHostBuilder Configure<TOptions>(this ISiloHostBuilder builder, IServiceCollection serviceProvider,IConfiguration configurationProvider) where TOptions : class
        {
            serviceProvider.AddOptions<TOptions>().Bind(configurationProvider);
            return builder;
        }

        public static ISiloHostBuilder ConfigureEndPointOptions(this ISiloHostBuilder builder, Func<HostBuilderContext, IConfiguration> hostBuilderContext)
        {
            return builder.ConfigureServices((context, collection) => {
                
                SiloEndPointOptions siloEndPointOptions = hostBuilderContext(context).Get<SiloEndPointOptions>();

                collection.Configure<Orleans.Configuration.EndpointOptions>(configureOptions => {


                    IPAddress ip = string.IsNullOrWhiteSpace(siloEndPointOptions.IP) ? Utilities.ResolveIPAddress(null, null, AddressFamily.InterNetwork) : IPAddress.Parse(siloEndPointOptions.IP);

                    configureOptions.SiloPort = siloEndPointOptions.SiloPort;
                    configureOptions.GatewayPort = siloEndPointOptions.GatewayPort;
                    configureOptions.AdvertisedIPAddress = ip;
                   

                    if (siloEndPointOptions.GatewayListeningProt > 0)
                    {
                        configureOptions.GatewayListeningEndpoint = new IPEndPoint(ip, siloEndPointOptions.GatewayListeningProt);
                    }

                    if (siloEndPointOptions.SiloListeningProt > 0)
                    {
                        configureOptions.SiloListeningEndpoint = new IPEndPoint(ip, siloEndPointOptions.SiloListeningProt);
                    }

                    if (siloEndPointOptions.ListenOnAnyHostAddress)
                    {
                        configureOptions.SiloListeningEndpoint = new IPEndPoint(IPAddress.Any, siloEndPointOptions.SiloPort);
                        configureOptions.GatewayListeningEndpoint = new IPEndPoint(IPAddress.Any, siloEndPointOptions.GatewayPort);
                    }
                });
                

               
              
            });
        }

        public static ISiloHostBuilder UseEnvironment(this ISiloHostBuilder builder, Func<HostBuilderContext, string> hostBuilderContext)
        {
            return builder.ConfigureServices((context, collection) => {
                builder.UseEnvironment(hostBuilderContext(context));
            });
        }
    }
}
