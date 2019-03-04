using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
namespace Leopard.Core.Startup
{
    public class StartupHelper : IStartup
    {
        private readonly StartupMethods _StartupMethods;

        public StartupHelper(StartupMethods methods)
        {
            this._StartupMethods = methods;
        }

        public void Configure(ISiloHostBuilder siloHostBuilder)
        {
            try
            {
                this._StartupMethods.ConfigureDelegate(siloHostBuilder);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }

                throw;
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                _StartupMethods.ConfigureServicesDelegate(services);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }

                throw;
            }
        }
    }
}
