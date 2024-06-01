using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Function")]
    public partial class Function
    {
        public Function()
        {
            RoleFunctions = new HashSet<RoleFunction>();
            AccountFunctions = new HashSet<AccountFunction>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string Area { get; set; }
        [Required]
        public string Controller { get; set; }
        [Required]
        public string Action { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsShow { get; set; }
        public bool? IsMenu { get; set; }
        public int? Seq { get; set; }
        public bool? IsDelete { get; set; }
        public long CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }
        [InverseProperty("Function")]
        public virtual ICollection<RoleFunction> RoleFunctions { get; set; }
        [InverseProperty("Function")]
        public virtual ICollection<AccountFunction> AccountFunctions { get; set; }
    }
}
