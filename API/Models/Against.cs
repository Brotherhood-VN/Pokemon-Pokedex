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
        [StringLength(50)]
        public string Zero { get; set; }
        [StringLength(50)]
        public string Quarter { get; set; }
        [StringLength(50)]
        public string Half { get; set; }
        [StringLength(50)]
        public string Two { get; set; }
        [StringLength(50)]
        public string Four { get; set; }
        [ForeignKey("PokemonId")]
        [InverseProperty("Againsts")]
        public virtual Pokemon Pokemon { get; set; }
    }
}