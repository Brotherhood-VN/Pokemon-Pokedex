using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Ability")]
    public partial class Ability
    {
        [Key]
        public long Id { get; set; }
        public long PokemonId { get; set; }
        public long? GenderId { get; set; }
        public long? AreaId { get; set; }
        public long? RegionId { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? Height { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? Weight { get; set; }
        [ForeignKey("PokemonId")]
        [InverseProperty("Abilitys")]
        public virtual Pokemon Pokemon { get; set; }
        [ForeignKey("GenderId")]
        [InverseProperty("Abilitys")]
        public virtual Gender Gender { get; set; }
        [ForeignKey("AreaId")]
        [InverseProperty("Abilitys")]
        public virtual Area Area { get; set; }
        [ForeignKey("RegionId")]
        [InverseProperty("Abilitys")]
        public virtual Region Region { get; set; }
    }
}
