namespace Profilum.AccountService.BLL.Models;

public class AccountRequest
{
    public long Id { get; set; }
   
    public long UserId { get; set; }
    
    public string AccountNumber { get; set; }
    
    public DAL.Models.AccountRequest ConvertToDal()
    {
        return new DAL.Models.AccountRequest
        {
            Id = Id,
            UserId = UserId,
            AccountNumber = AccountNumber
        };
    }
}