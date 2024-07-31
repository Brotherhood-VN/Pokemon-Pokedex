using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Breeding")]
    public partial class Breeding
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100)]
        public string EggGroup { get; set; }
        [StringLength(250)]
        public string GenderDistribution { get; set; }
        [StringLength(250)]
        public string EggCycle { get; set; }
        public long PokemonId { get; set; }
    }
}
