using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("RoleFunction")]
    public partial class RoleFunction
    {
        [Key]
        public long Id { get; set; }
        public long RoleId { get; set; }
        public long FunctionId { get; set; }
        [ForeignKey("RoleId")]
        [InverseProperty("RoleFunctions")]
        public virtual Role Role { get; set; }
        [ForeignKey("FunctionId")]
        [InverseProperty("RoleFunctions")]
        public virtual Function Function { get; set; }
    }
}
