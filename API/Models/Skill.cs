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
        public long Create_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Create_Time { get; set; }
        public long? Update_By { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Update_Time { get; set; }
        [ForeignKey("ItemId")]
        [InverseProperty("Skills")]
        public virtual Item Item { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<PokemonSkill> PokemonSkills { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<SkillCondition> SkillConditions { get; set; }
    }
}
