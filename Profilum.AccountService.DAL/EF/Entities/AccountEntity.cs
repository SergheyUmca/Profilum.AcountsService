using System.ComponentModel.DataAnnotations.Schema;

namespace Profilum.AccountService.DAL.EF.Entities
{
    
    [Table("Account", Schema = "Account")]
    public class AccountEntity
    {
        
        public long Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public string AccountNumber { get; set; }
    }
}
