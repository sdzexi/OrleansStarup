using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Core.Startup
{
    public class StartupMethods
    {
        public object StartupInstance { get; private set; }

        public Action<ISiloHostBuilder> ConfigureDelegate { get; private set; }

        public Action<IServiceCollection> ConfigureServicesDelegate { get; private set; }
        public StartupMethods(object instance, Action<ISiloHostBuilder> configure, Action<IServiceCollection> configureServices)
        {
            this.StartupInstance = instance;
            this.ConfigureDelegate = configure;
            this.ConfigureServicesDelegate = configureServices;
        }



    }
}
