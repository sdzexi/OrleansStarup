using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.ApplicationParts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Core.Hosting
{
    public interface IOrleansHostBuilder
    {
        IOrleansHostBuilder ConfigureApplicationParts(Action<IApplicationPartManager> configure);

        IOrleansHostBuilder ConfigureServices(Action<IServiceCollection> configureServices);

        IOrleansHost Build();
    }
}
