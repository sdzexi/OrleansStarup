using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leopard.Core.Startup
{
    public interface IStartup
    {
        void Configure(ISiloHostBuilder app);
        void ConfigureServices(IServiceCollection services);
    }
}
