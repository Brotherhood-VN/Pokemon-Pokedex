using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Skill")]
    public partial class Skill
    {
        public Skill()
        {
            PokemonSkills = new HashSet<PokemonSkill>();
            SkillConditions = new HashSet<SkillCondition>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int? Level { get; set; }
        public long? ItemId { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
        public long CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }
        [ForeignKey("ItemId")]
        [InverseProperty("Skills")]
        public virtual Item Item { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<PokemonSkill> PokemonSkills { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<SkillCondition> SkillConditions { get; set; }
    }
}
