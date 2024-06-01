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
            Abilitys = new HashSet<Ability>();
        }

        [Key]
        public long Id { get; set; }
        public long AreaId { get; set; }
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
        [ForeignKey("AreaId")]
        [InverseProperty("Regions")]
        public virtual Area Area { get; set; }
        [InverseProperty("Region")]
        public virtual ICollection<Stat> Stats { get; set; }
        [InverseProperty("Region")]
        public virtual ICollection<Ability> Abilitys { get; set; }
    }
}
