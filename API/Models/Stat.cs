using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Stat")]
    public partial class Stat
    {
        [Key]
        public long Id { get; set; }
        public long PokemonId { get; set; }
        public long? AreaId { get; set; }
        public long? RegionId { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }
        public int Speed { get; set; }
        public int SpeedAttack { get; set; }
        public int SpeedDefence { get; set; }
        public long StatTypeId { get; set; }
        [ForeignKey("AreaId")]
        [InverseProperty("Stats")]
        public virtual Area Area { get; set; }
        [ForeignKey("RegionId")]
        [InverseProperty("Stats")]
        public virtual Region Region { get; set; }
        [ForeignKey("PokemonId")]
        [InverseProperty("Stats")]
        public virtual Pokemon Pokemon { get; set; }
        [ForeignKey("StatTypeId")]
        [InverseProperty("Stats")]
        public virtual StatType StatType { get; set; }
    }
}
