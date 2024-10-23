using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("GameVersion")]
    public partial class GameVersion
    {
        public GameVersion()
        {
            SkillGameVersions = new HashSet<SkillGameVersion>();
            PokemonGameVersions = new HashSet<PokemonGameVersion>();
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
        [InverseProperty("GameVersion")]
        public virtual ICollection<SkillGameVersion> SkillGameVersions { get; set; }
        [InverseProperty("GameVersion")]
        public virtual ICollection<PokemonGameVersion> PokemonGameVersions { get; set; }
    }
}
