using Grpc.Core;
using Profilum.AccountService.BLL.Handlers.Interfaces;
using Profilum.AccountService.BLL.Models;
using static Profilum.AccountService.AccountService;

namespace Profilum.AccountService.Api.Services;

public class AccountService : AccountServiceBase
{
    private readonly IAccountHandler _accountHandler;

     public AccountService( IAccountHandler accountHandler)
     {
            _accountHandler = accountHandler;
     }
        
     public override async Task<AccountFullReply> GetAccount(AccountGetRequest request, ServerCallContext context)
     {
         var getAccount = await _accountHandler.Get(request.Id);
         if(!getAccount.IsSuccess)
         {
             //set statusCode
         }
        
         return new AccountFullReply
         {
             Id = getAccount.Data.Id,
             UserId = getAccount.Data.UserId.ToString(),
             AccountNumber = getAccount.Data.AccountNumber,
             ReplyStateCode = (int)getAccount.ResultCode
         };
     }
     
     public override async Task GetAllAccounts(EmptyRequest request , IServerStreamWriter<AccountFullReply> responseStream,
         ServerCallContext context)
     {
         await foreach (var accountResponse in _accountHandler.GetAllStream())
         {
             await responseStream.WriteAsync(new AccountFullReply
             {
                 Id = accountResponse.Id,
                UserId = accountResponse.UserId.ToString(),
                AccountNumber = accountResponse.AccountNumber
             });
         }
     }
    
     public override async Task<AccountFullReply> CreateAccount(AccountCreateRequest request, ServerCallContext context)
     {
         var crateAccount = await _accountHandler.Create(new AccountRequest
         {
             AccountNumber = request.AccountNumber,
             UserId = Guid.Parse(request.UserId)
         });
         if(!crateAccount.IsSuccess)
         {
             //set statusCode
         }
        
         return new AccountFullReply
         {
             Id = crateAccount.Data.Id,
             UserId = crateAccount.Data.UserId.ToString(),
             AccountNumber = crateAccount.Data.AccountNumber,
             ReplyStateCode = (int)crateAccount.ResultCode
         };
     }
     
     public override async Task<AccountFullReply> UpdateAccount(AccountCreateRequest request, ServerCallContext context)
     {
         var updateAccount = await _accountHandler.Update(new AccountRequest
         {
             Id = request.Id,
             AccountNumber = request.AccountNumber,
             UserId = Guid.Parse(request.UserId)
         });
         if(!updateAccount.IsSuccess)
         {
             //set statusCode
         }
        
         return new AccountFullReply
         {
             Id = updateAccount.Data.Id,
             UserId = updateAccount.Data.UserId.ToString(),
             AccountNumber = updateAccount.Data.AccountNumber,
             ReplyStateCode = (int)updateAccount.ResultCode
         };
     }
      
     public override async Task<EmptyReply> DeleteAccount(AccountGetRequest request, ServerCallContext context)
     {
         var deleteAccount = await _accountHandler.Delete(request.Id);
         if(!deleteAccount.IsSuccess)
         {
             //set statusCode
         }
        
         return new EmptyReply
         {
             ReplyStateCode = (int)deleteAccount.ResultCode
         };
     }
     
     public override async Task<EmptyReply> DeleteAllAccounts(EmptyRequest request, ServerCallContext context)
     {
         var deleteAllAccounts = await _accountHandler.DeleteAll();
         if(!deleteAllAccounts.IsSuccess)
         {
             //set statusCode
         }
        
         return new EmptyReply
         {
             ReplyStateCode = (int)deleteAllAccounts.ResultCode
         };
     }
}