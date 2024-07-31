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
        [StringLength(250)]
        public string Area { get; set; }
        [Required]
        [StringLength(250)]
        public string Controller { get; set; }
        [Required]
        [StringLength(250)]
        public string Action { get; set; }
        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(250)]
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
