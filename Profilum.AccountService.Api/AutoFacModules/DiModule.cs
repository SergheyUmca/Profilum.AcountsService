using Autofac;
using Autofac.Core;
using Profilum.AccountService.BLL.Handlers.Implementations;
using Profilum.AccountService.BLL.Handlers.Interfaces;
using Profilum.AccountService.Common;

namespace Profilum.AccountService.Api.AutoFacModules
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DiModule : ConfiguredModule
    {
        public DiModule(AppSettings settings) : base(settings)
        {
            
        }
        protected override void Load(ContainerBuilder builder)
        {
            var parameters = new List<Parameter>
            {
                new NamedParameter("connectionString", Settings.ConnectionString),
                new NamedParameter("dbName", Settings.Database)
            };
            
            builder.RegisterType<Services.AccountService>().As<AccountService.AccountServiceBase>();
            
            builder.RegisterType<AccountHandler>().As<IAccountHandler>().WithParameters(parameters);
            
            builder.RegisterType<GrpcServer>().As<IHostedService>().InstancePerDependency()
                .WithParameter(new NamedParameter("settings", Settings));
            builder.RegisterType<KafkaConsumerServer>().As<IHostedService>().InstancePerDependency()
                .WithParameter(new NamedParameter("settings", Settings));
        }

        
    }
}