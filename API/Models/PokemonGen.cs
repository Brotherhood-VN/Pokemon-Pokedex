using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("PokemonGen")]
    public partial class PokemonGen
    {
        [Key]
        public long Id { get; set; }
        public long PokemonId { get; set; }
        public long GenerationId { get; set; }
        [ForeignKey("PokemonId")]
        [InverseProperty("PokemonGens")]
        public virtual Pokemon Pokemon { get; set; }
        [ForeignKey("GenerationId")]
        [InverseProperty("PokemonGens")]
        public virtual Generation Generation { get; set; }
    }
}
