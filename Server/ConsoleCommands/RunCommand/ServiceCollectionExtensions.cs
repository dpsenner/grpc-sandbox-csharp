using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GrpcSandbox.Server.ConsoleCommands.RunCommand
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllImplementationsOfInterfaceInAssemblyAsSingleton<TInterface>(this IServiceCollection serviceCollection, Assembly assembly)
        {
            Type interfaceType = typeof(TInterface);
            if (!interfaceType.IsInterface)
            {
                throw new InvalidOperationException($"Not an interface: {interfaceType}");
            }

            foreach (var implementationType in assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && interfaceType.IsAssignableFrom(t)))
            {
                serviceCollection.AddSingleton(interfaceType, implementationType);
            }

            return serviceCollection;
        }
    }
}
