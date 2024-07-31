using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("PokemonClass")]
    public partial class PokemonClass
    {
        [Key]
        public long Id { get; set; }
        public long PokemonId { get; set; }
        public long ClassificationId { get; set; }
        public bool IsDefault { get; set; }
        [ForeignKey("ClassificationId")]
        [InverseProperty("PokemonClasses")]
        public virtual Classification Classification { get; set; }
        [ForeignKey("PokemonId")]
        [InverseProperty("PokemonClasses")]
        public virtual Pokemon Pokemon { get; set; }
    }
}
