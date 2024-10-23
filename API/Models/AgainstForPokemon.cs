using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("AgainstForPokemon")]
    public partial class AgainstForPokemon
    {
        [Key]
        public long Id { get; set; }
        public long PokemonId { get; set; }
        [StringLength(500)]
        public string Zero { get; set; }
        [StringLength(500)]
        public string Quarter { get; set; }
        [StringLength(500)]
        public string Half { get; set; }
        [StringLength(500)]
        public string Two { get; set; }
        [StringLength(500)]
        public string Four { get; set; }
    }
}
