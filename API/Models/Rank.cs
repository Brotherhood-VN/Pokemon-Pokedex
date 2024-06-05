using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Rank")]
    public partial class Rank
    {
        public Rank()
        {
            Pokemons = new HashSet<Pokemon>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
        public long CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }
        [InverseProperty("Rank")]
        public virtual ICollection<Pokemon> Pokemons { get; set; }
    }
}
