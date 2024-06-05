using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Gender")]
    public partial class Gender
    {
        public Gender()
        {
            Abilitys = new HashSet<Ability>();
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
        public long CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }
        [InverseProperty("Gender")]
        public virtual ICollection<Ability> Abilitys { get; set; }
    }
}
