using Profilum.AccountService.BLL.Models;
using static Profilum.AccountService.Common.BaseModels.AppResponse;

namespace Profilum.AccountService.BLL.Handlers.Interfaces;

public interface IAccountHandler
{
    Task<Response<AccountResponse>> Get(long id);

    Task<Response<List<AccountResponse>>> GetAll();

    IAsyncEnumerable<AccountResponse> GetAllStream();

    Task<Response<AccountResponse>> Create(AccountRequest request);

    Task<Response<AccountResponse>> Update(AccountRequest request);

    Task<Response> Delete(long id);

    Task<Response> DeleteAll();
}