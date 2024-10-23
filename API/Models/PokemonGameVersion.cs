using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("PokemonGameVersion")]
    public partial class PokemonGameVersion
    {
        [Key]
        public long Id { get; set; }
        public long PokemonId { get; set; }
        public long GameVersionId { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [ForeignKey("PokemonId")]
        [InverseProperty("PokemonGameVersions")]
        public virtual Pokemon Pokemon { get; set; }
        [ForeignKey("GameVersionId")]
        [InverseProperty("PokemonGameVersions")]
        public virtual GameVersion GameVersion { get; set; }
    }
}
