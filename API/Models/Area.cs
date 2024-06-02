using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Area")]
    public partial class Area
    {
        public Area()
        {
            Regions = new HashSet<Region>();
            Stats = new HashSet<Stat>();
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
        public long Create_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Create_Time { get; set; }
        public long? Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }
        [InverseProperty("Area")]
        public virtual ICollection<Region> Regions { get; set; }
        [InverseProperty("Area")]
        public virtual ICollection<Stat> Stats { get; set; }
        [InverseProperty("Area")]
        public virtual ICollection<Ability> Abilitys { get; set; }
    }
}