using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Pokemon")]
    public partial class Pokemon
    {
        public Pokemon()
        {
            PokemonSkills = new HashSet<PokemonSkill>();
            Abilitys = new HashSet<Ability>();
            Stats = new HashSet<Stat>();
            PokemonClasss = new HashSet<PokemonClass>();
            PokemonGens = new HashSet<PokemonGen>();
            Againsts = new HashSet<Against>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string Index { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Story { get; set; }
        public string Note { get; set; }
        public string BeforeIndex { get; set; }
        public long? RankId { get; set; }
        public long? ConditionId { get; set; }
        public int? Level { get; set; }
        public long? StoneId { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
        public long CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }
        [ForeignKey("StoneId")]
        [InverseProperty("Pokemons")]
        public virtual Stone Stone { get; set; }
        [ForeignKey("RankId")]
        [InverseProperty("Pokemons")]
        public virtual Rank Rank { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<PokemonSkill> PokemonSkills { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<Ability> Abilitys { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<Stat> Stats { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<PokemonClass> PokemonClasss { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<PokemonGen> PokemonGens { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<Against> Againsts { get; set; }
    }
}
