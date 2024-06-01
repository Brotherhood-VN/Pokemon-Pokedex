using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("PokemonSkill")]
    public partial class PokemonSkill
    {
        [Key]
        public long Id { get; set; }
        public long PokemonId { get; set; }
        public long SkillId { get; set; }
        [ForeignKey("PokemonId")]
        [InverseProperty("PokemonSkills")]
        public virtual Pokemon Pokemon { get; set; }
        [ForeignKey("SkillId")]
        [InverseProperty("PokemonSkills")]
        public virtual Skill Skill { get; set; }
    }
}
