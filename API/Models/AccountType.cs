using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("AccountType")]
    public partial class AccountType
    {
        public AccountType()
        {
            Accounts = new HashSet<Account>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
        public long Create_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Create_Time { get; set; }
        public long? Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }
        [InverseProperty("AccountType")]
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
