namespace Profilum.AccountService.Api.Models;

public class AccountRequest
{
    public long Id { get; set; }
   
    public Guid UserId { get; set; }
    
    public string AccountNumber { get; set; }
    
    public BLL.Models.AccountRequest ConvertToBll()
    {
        return new BLL.Models.AccountRequest
        {
            Id = Id,
            UserId = UserId,
            AccountNumber = AccountNumber
        };
    }
}