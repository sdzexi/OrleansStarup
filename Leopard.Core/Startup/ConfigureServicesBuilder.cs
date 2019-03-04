using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Leopard.Core.Startup
{
    public class ConfigureServicesBuilder
    {
        public MethodInfo MethodInfo { get; }
        public ConfigureServicesBuilder(MethodInfo configureServices)
        {
            if (configureServices == null)
            {
                throw new ArgumentNullException(nameof(configureServices));
            }

            // Only support IServiceCollection parameters
            var parameters = configureServices.GetParameters();
            if (parameters.Length > 1 ||
                parameters.Any(p => p.ParameterType != typeof(IServiceCollection)))
            {
                throw new InvalidOperationException("ConfigureServices can take at most a single IServiceCollection parameter.");
            }

            MethodInfo = configureServices;
        }

        public Action<IServiceCollection> Build(object instance)
        {
            return (serviceCollection) => {
                this.MethodInfo.Invoke(instance, new object[] { serviceCollection });
            };
        }
    }
}
