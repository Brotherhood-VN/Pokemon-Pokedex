using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Pokemon")]
    public partial class Pokemon
    {
        public Pokemon()
        {
            PokemonAbilities = new HashSet<PokemonAbility>();
            PokemonGameVersions = new HashSet<PokemonGameVersion>();
            Againsts = new HashSet<Against>();
            PokemonGens = new HashSet<PokemonGen>();
            PokemonClasses = new HashSet<PokemonClass>();
            PokemonSkills = new HashSet<PokemonSkill>();
            Stats = new HashSet<Stat>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Index { get; set; }
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? Height { get; set; }
        [Column(TypeName = "decimal(7, 2)")]
        public decimal? Weight { get; set; }
        public string Description { get; set; }
        public string Story { get; set; }
        public string Note { get; set; }
        [StringLength(50)]
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
        public virtual ICollection<PokemonAbility> PokemonAbilities { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<PokemonGameVersion> PokemonGameVersions { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<Against> Againsts { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<PokemonGen> PokemonGens { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<PokemonClass> PokemonClasses { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<PokemonSkill> PokemonSkills { get; set; }
        [InverseProperty("Pokemon")]
        public virtual ICollection<Stat> Stats { get; set; }
    }
}
