using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Generation")]
    public partial class Generation
    {
        public Generation()
        {
            Skills = new HashSet<Skill>();
            PokemonGens = new HashSet<PokemonGen>();
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
        [InverseProperty("Generation")]
        public virtual ICollection<Skill> Skills { get; set; }
        [InverseProperty("Generation")]
        public virtual ICollection<PokemonGen> PokemonGens { get; set; }
    }
}
