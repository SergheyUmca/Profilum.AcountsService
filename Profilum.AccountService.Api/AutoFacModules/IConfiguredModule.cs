using Profilum.AccountService.Api.Models;
using Profilum.AccountService.Common;

namespace Profilum.AccountService.Api.AutoFacModules
{
    public interface IConfiguredModule
    {
        AppSettings Settings { get; set; }
    }
}