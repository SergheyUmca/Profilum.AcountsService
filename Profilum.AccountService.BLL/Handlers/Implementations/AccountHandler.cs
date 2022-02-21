using Profilum.AccountService.BLL.Handlers.Interfaces;
using Profilum.AccountService.BLL.Models;
using Profilum.AccountService.Common.BaseModels;
using Profilum.AccountService.DAL.EF.Services.Implementations;
using Profilum.AccountService.DAL.EF.Services.Interfaces;
using static Profilum.AccountService.Common.BaseModels.AppResponse;

namespace Profilum.AccountService.BLL.Handlers.Implementations;

public class AccountHandler : IAccountHandler
{

    private readonly string _connectionString;
    public AccountHandler(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<Response<AccountResponse>> Get(long id)
    {
        try
        {
            DAL.Models.AccountResponse dbAccount;
            using (IDbService dbService = new DbService(_connectionString).DbServiceInstance)
            {
                var getAccount = await dbService.Accounts.Get(id);
                if (!getAccount.IsSuccess)
                    throw new CustomException(getAccount.ResultCode, getAccount.LastResultMessage);

                dbAccount = getAccount.Data;
            }
            
            return new Response<AccountResponse>(new AccountResponse(dbAccount));
        }
        catch (CustomException ce)
        {
            return new ErrorResponse<AccountResponse>(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse<AccountResponse>(e.Message);
        }
    }
    
    public async Task<Response<List<AccountResponse>>> GetAll()
    {
        try
        {
            List<DAL.Models.AccountResponse> dbAccounts;
            using (IDbService dbService = new DbService(_connectionString).DbServiceInstance)
            {
                var getAllAccounts = await dbService.Accounts.GetAll();
                if (!getAllAccounts.IsSuccess)
                {
                    return getAllAccounts.IsCriticalCode
                        ? throw new CustomException(getAllAccounts.ResultCode, getAllAccounts.LastResultMessage)
                        : new Response<List<AccountResponse>>(new List<AccountResponse>());
                }

                dbAccounts = getAllAccounts.Data;

            }

            return new Response<List<AccountResponse>>(dbAccounts.Select(u => new AccountResponse(u)).ToList());
        }
        catch (CustomException ce)
        {
            return new ErrorResponse<List<AccountResponse>>(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse<List<AccountResponse>>(e.Message);
        }
    }
    
    public async IAsyncEnumerable<AccountResponse> GetAllStream()
    {
        // ReSharper disable once ConvertToUsingDeclaration
        using (IDbService dbService = new DbService(_connectionString).DbServiceInstance)
        {
            var getAccount =  dbService.Accounts.GetAllAsyncEnumerable();
            await foreach (var accountResponse in getAccount) 
                yield return new AccountResponse(accountResponse);
        }
    }

    public async Task<Response<AccountResponse>> Create(AccountRequest request)
    {
        try
        {
            long id;
            using (IDbService dbService = new DbService(_connectionString).DbServiceInstance)
            {
                var createAccount = await dbService.Accounts.Create(request.ConvertToDal());
                if (!createAccount.IsSuccess)
                    throw new CustomException(createAccount.ResultCode, createAccount.LastResultMessage);

                id = createAccount.Data;
            }

            return new Response<AccountResponse>(new AccountResponse
            {
                Id = id,
                UserId = request.UserId,
                AccountNumber = request.AccountNumber
            });
        }
        catch (CustomException ce)
        {
            return new ErrorResponse<AccountResponse>(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse<AccountResponse>(e.Message);
        }
    }
    
    public async Task<Response<AccountResponse>> Update(AccountRequest request)
    {
        try
        {
            using (IDbService dbService = new DbService(_connectionString).DbServiceInstance)
            {
                var updateAccount = await dbService.Accounts.Update(request.ConvertToDal());
                if (!updateAccount.IsSuccess)
                    throw new CustomException(updateAccount.ResultCode, updateAccount.LastResultMessage);
            }

            return new Response<AccountResponse>(new AccountResponse
            {
                Id = request.Id,
                UserId = request.UserId,
                AccountNumber = request.AccountNumber
            });
        }
        catch (CustomException ce)
        {
            return new ErrorResponse<AccountResponse>(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse<AccountResponse>(e.Message);
        }
    }
    
    public async Task<Response> Delete(long id)
    {
        try
        {
            using (IDbService dbService = new DbService(_connectionString).DbServiceInstance)
            {
                var deleteAccount = await dbService.Accounts.Delete(id);
                if (!deleteAccount.IsSuccess)
                    throw new CustomException(deleteAccount.ResultCode, deleteAccount.LastResultMessage);
            }

            return new Response();
        }
        catch (CustomException ce)
        {
            return new ErrorResponse(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse(e.Message);
        }
    }
    
    public async Task<Response> DeleteAll()
    {
        try
        {
            using (IDbService dbService = new DbService(_connectionString).DbServiceInstance)
            {
                var deleteAllAccounts = await dbService.Accounts.DeleteAll();
                if (!deleteAllAccounts.IsSuccess)
                    throw new CustomException(deleteAllAccounts.ResultCode, deleteAllAccounts.LastResultMessage);
            }
            
            return new Response();
        }
        catch (CustomException ce)
        {
            return new ErrorResponse(ce.LastErrorMessage, ce.LastResultCode);
        }
        catch (Exception e)
        {
            return new ErrorResponse(e.Message);
        }
    }
}