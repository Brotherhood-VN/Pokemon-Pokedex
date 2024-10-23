using System;
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
        [StringLength(10)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
        public long CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        [InverseProperty("Role")]
        public virtual ICollection<RoleFunction> RoleFunctions { get; set; }
    }
}
