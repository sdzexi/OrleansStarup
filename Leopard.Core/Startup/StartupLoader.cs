using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Leopard.Core.Startup
{
    public class StartupLoader
    {
        public static StartupMethods LoadMethods(IServiceProvider hostingServiceProvider, Type startupType)
        {
            ConfigureServicesBuilder servicesMethod = null;
            ConfigureBuilder configureContainerMethod = null;

            servicesMethod = FindConfigureServicesDelegate(startupType);
            configureContainerMethod = FindConfigureDelegate(startupType);

            object instance = null;
            if ((servicesMethod != null && !servicesMethod.MethodInfo.IsStatic) || (configureContainerMethod != null && !configureContainerMethod.MethodInfo.IsStatic))
            {
                instance = ActivatorUtilities.GetServiceOrCreateInstance(hostingServiceProvider, startupType);
            }


            return new StartupMethods(instance, configureContainerMethod.Build(instance), servicesMethod.Build(instance));
        }

        private static ConfigureBuilder FindConfigureDelegate(Type startupType)
        {
            var servicesMethod = FindMethod(startupType, "Configure", typeof(IServiceCollection), false);

            return servicesMethod == null ? null : new ConfigureBuilder(servicesMethod);
        }
        private static ConfigureServicesBuilder FindConfigureServicesDelegate(Type startupType)
        {
            var servicesMethod = FindMethod(startupType, "ConfigureServices", typeof(IServiceProvider), false) ?? FindMethod(startupType, "ConfigureServices", typeof(void), false);

            return servicesMethod == null ? null : new ConfigureServicesBuilder(servicesMethod);
        }

        public static MethodInfo FindMethod(Type startupType, string methodName, Type paramType, bool required)
        {
            var methods = startupType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            var selectedMethods = methods.Where(method => method.Name.Equals(methodName)).ToList();

            if (selectedMethods.Count > 1)
            {
                throw new InvalidOperationException($"Having multiple overloads of method '{methodName}' is not supported.");
            }

            var methodInfo = selectedMethods.FirstOrDefault();

            if (methodInfo == null)
            {
                if (required)
                {
                    throw new InvalidOperationException($"A method named '{methodName}' in the type '{startupType.FullName}' could not be found.");
                }

                return null;
            }

            return methodInfo;
        }
    }
}
