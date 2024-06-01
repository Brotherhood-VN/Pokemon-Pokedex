using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            AccountRoles = new HashSet<AccountRole>();
            AccountFunctions = new HashSet<AccountFunction>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool? Status { get; set; }
        public bool? IsDelete { get; set; }
        public long CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }
        public long AccountTypeId { get; set; }
        [ForeignKey("AccountTypeId")]
        [InverseProperty("Accounts")]
        public virtual AccountType AccountType { get; set; }
        [InverseProperty("Account")]
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        [InverseProperty("Account")]
        public virtual ICollection<AccountFunction> AccountFunctions { get; set; }
    }
}
