using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Area")]
    public partial class Area
    {
        public Area()
        {
            Stats = new HashSet<Stat>();
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
        public long RegionId { get; set; }
        [ForeignKey("RegionId")]
        [InverseProperty("Areas")]
        public virtual Region Region { get; set; }
        [InverseProperty("Area")]
        public virtual ICollection<Stat> Stats { get; set; }
    }
}
