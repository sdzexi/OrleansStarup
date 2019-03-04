using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.ApplicationParts;

namespace Leopard.Core.Hosting
{
    public class OrleansHostBuilder : IOrleansHostBuilder
    {

        public List<Action<IServiceCollection>> _configureServicesDelegates { get; set; }

        public ISiloHostBuilder _SiloHostBuilder { get; set; }

        public OrleansHostBuilder(ISiloHostBuilder siloHostBuilder)
        {
            this._SiloHostBuilder = siloHostBuilder;
            this._configureServicesDelegates = new List<Action<IServiceCollection>>();
        }

        public IOrleansHostBuilder ConfigureApplicationParts(Action<IApplicationPartManager> configure)
        {
            this._SiloHostBuilder.ConfigureApplicationParts(configure);

            return this;
        }

        public IOrleansHost Build()
        {
            this._SiloHostBuilder.ConfigureServices((hostBuilderContext, serviceProvider)=> {
                serviceProvider = BuildCommonServices(hostBuilderContext,serviceProvider);
                ServiceProvider hostingServiceProvider = serviceProvider.BuildServiceProvider();
                Startup(hostingServiceProvider, serviceProvider);
            });
            return (IOrleansHost) new SiloHost(this._SiloHostBuilder.Build());
        }

        public IOrleansHostBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            this._configureServicesDelegates.Add(configureServices);

            return (IOrleansHostBuilder)this;
        }


        private IServiceCollection BuildCommonServices(HostBuilderContext hostBuilderContext,IServiceCollection servicesProvderDelegate)
        {
           //加入配置
            servicesProvderDelegate.AddSingleton<IConfiguration>(hostBuilderContext.Configuration);

            foreach (Action<IServiceCollection> servicesDelegate in this._configureServicesDelegates)
                servicesDelegate((IServiceCollection)servicesProvderDelegate);
            
            servicesProvderDelegate.AddSingleton<Microsoft.AspNetCore.Hosting.IHostingEnvironment, HostingEnvironment>();
            servicesProvderDelegate.AddTransient<IServiceProviderFactory<IServiceCollection>, DefaultServiceProviderFactory>();

            return servicesProvderDelegate;
        }

        private void Startup(ServiceProvider hostingServiceProvider, IServiceCollection hostingServices)
        {
            Leopard.Core.Startup.IStartup startUp = hostingServiceProvider.GetService<Leopard.Core.Startup.IStartup>();

            if (startUp != null)
            {
                startUp.ConfigureServices(hostingServices);
                startUp.Configure(this._SiloHostBuilder);
            }
            hostingServiceProvider = hostingServices.BuildServiceProvider();
        }

        
    }
}
