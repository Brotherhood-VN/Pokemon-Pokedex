using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("AccountRole")]
    public partial class AccountRole
    {
        [Key]
        public long Id { get; set; }
        public long AccountId { get; set; }
        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("AccountRoles")]
        public virtual Role Role { get; set; }
        [ForeignKey("AccountId")]
        [InverseProperty("AccountRoles")]
        public virtual Account Account { get; set; }
    }
}
