using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("PokemonAbility")]
    public partial class PokemonAbility
    {
        [Key]
        public long Id { get; set; }
        public long PokemonId { get; set; }
        public long AbilityId { get; set; }
        [ForeignKey("PokemonId")]
        [InverseProperty("PokemonAbilities")]
        public virtual Pokemon Pokemon { get; set; }
        [ForeignKey("AbilityId")]
        [InverseProperty("PokemonAbilities")]
        public virtual Ability Ability { get; set; }
    }
}
