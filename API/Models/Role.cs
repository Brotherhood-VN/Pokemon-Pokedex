using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            AccountRoles = new HashSet<AccountRole>();
            RoleFunctions = new HashSet<RoleFunction>();
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
        [InverseProperty("Role")]
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<RoleFunction> RoleFunctions { get; set; }
    }
}
