using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Against")]
    public partial class Against
    {
        [Key]
        public long Id { get; set; }
        public long PokemonId { get; set; }
        [Required]
        public string Good { get; set; }
        [Required]
        public string Weak { get; set; }
        [ForeignKey("PokemonId")]
        [InverseProperty("Againsts")]
        public virtual Pokemon Pokemon { get; set; }
    }
}
