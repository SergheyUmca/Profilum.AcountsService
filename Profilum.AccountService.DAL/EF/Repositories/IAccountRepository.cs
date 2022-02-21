using Profilum.AccountService.DAL.Models;
using static Profilum.AccountService.Common.BaseModels.AppResponse;

namespace Profilum.AccountService.DAL.EF.Repositories
{
    public interface IAccountRepository
    {
        Task<Response<AccountResponse>> Get(long id);

        Task<Response<List<AccountResponse>>> GetAll();

        IAsyncEnumerable<AccountResponse> GetAllAsyncEnumerable();

        Task<Response<long>> Create(AccountRequest request);

        Task<Response> Update(AccountRequest entity);

        Task<Response> Delete(long id);

        Task<Response> DeleteAll();
    }
}