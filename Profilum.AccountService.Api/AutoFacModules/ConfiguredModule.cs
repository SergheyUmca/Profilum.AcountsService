using Autofac;
using Profilum.AccountService.Api.Models;
using Profilum.AccountService.Common;

namespace Profilum.AccountService.Api.AutoFacModules
{
    public abstract class ConfiguredModule : Module, IConfiguredModule
    {
        protected ConfiguredModule(AppSettings settings)
        {
            Settings = settings;
        }

        public AppSettings Settings { get; set; }
    }
}