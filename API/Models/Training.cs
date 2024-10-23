using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Training")]
    public partial class Training
    {
        [Key]
        public long Id { get; set; }
        [StringLength(250)]
        public string EVYield { get; set; }
        [StringLength(250)]
        public string CatchRate { get; set; }
        [StringLength(250)]
        public string Base { get; set; }
        [StringLength(250)]
        public string FriendShip { get; set; }
        [StringLength(250)]
        public string BaseExp { get; set; }
        [StringLength(250)]
        public string GrowthRate { get; set; }
        [StringLength(250)]
        public string HeldItem { get; set; }
        public long PokemonId { get; set; }
    }
}
