namespace Profilum.AccountService.Api.Models;

public class AccountResponse
{
   public long Id { get; set; }
   
   public Guid UserId { get; set; }
    
   public string AccountNumber { get; set; }
   
   public AccountResponse(BLL.Models.AccountResponse bllResponse)
   {
      Id = bllResponse.Id;
      UserId = bllResponse.UserId;
      AccountNumber = bllResponse.AccountNumber;
   }
}