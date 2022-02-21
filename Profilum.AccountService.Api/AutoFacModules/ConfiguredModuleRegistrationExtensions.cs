using System.Reflection;
using Autofac;
using Autofac.Core;
using Profilum.AccountService.Api.Models;
using Profilum.AccountService.Common;

namespace Profilum.AccountService.Api.AutoFacModules
{
    public static class ConfiguredModuleRegistrationExtensions
    {
        public static void RegisterConfiguredModulesFromAssemblyContaining<TType>(this ContainerBuilder builder,
            AppSettings settings)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var metaBuilder = new ContainerBuilder();

            metaBuilder.RegisterInstance(settings);
            metaBuilder.RegisterAssemblyTypes(typeof(TType).GetTypeInfo().Assembly)
                .AssignableTo<IModule>()
                .As<IModule>()
                .PropertiesAutowired();

            // ReSharper disable once ConvertToUsingDeclaration
            using (var metaContainer = metaBuilder.Build())
            {
                foreach (var module in metaContainer.Resolve<IEnumerable<IModule>>())
                    builder.RegisterModule(module);
            }
        }
    }
}