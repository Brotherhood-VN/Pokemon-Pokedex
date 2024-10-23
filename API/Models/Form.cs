using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Form")]
    public partial class Form
    {
        [Key]
        public long Id { get; set; }
        public bool AlternativeForm { get; set; }
        public bool GenderDifference { get; set; }
        public long PokemonId { get; set; }
    }
}
