using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Region")]
    public partial class Region
    {
        public Region()
        {
            Stats = new HashSet<Stat>();
        }

        [Key]
        public long Id { get; set; }
        public long AreaId { get; set; }
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
        [ForeignKey("AreaId")]
        [InverseProperty("Regions")]
        public virtual Area Area { get; set; }
        [InverseProperty("Region")]
        public virtual ICollection<Stat> Stats { get; set; }
    }
}
