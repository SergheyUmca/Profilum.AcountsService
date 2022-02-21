
namespace Profilum.AccountService.DAL.Models;

public class AccountResponse
{
   public long Id { get; set; }
   
   public Guid UserId { get; set; }
    
   public string AccountNumber { get; set; }

   public AccountResponse()
   {
      
   }
   
   public AccountResponse(AccountRequest request)
   {
      Id = request.Id;
      UserId = request.UserId;
      AccountNumber = request.AccountNumber;
   }
}