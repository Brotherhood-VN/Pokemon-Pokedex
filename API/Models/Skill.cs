using System;
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
            SkillGameVersions = new HashSet<SkillGameVersion>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Code { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        [StringLength(500)]
        public string Effect { get; set; }
        [StringLength(500)]
        public string InDepthEffect { get; set; }
        public int? Level { get; set; }
        public long? ItemId { get; set; }
        public bool? IsEgg { get; set; }
        public bool? IsTutor { get; set; }
        public int? Power { get; set; }
        public int? Accuracy { get; set; }
        public int? PP { get; set; }
        public int? Priority { get; set; }
        public long? GenerationId { get; set; }
        [StringLength(50)]
        public string Classes { get; set; }
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
        [ForeignKey("GenerationId")]
        [InverseProperty("Skills")]
        public virtual Generation Generation { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<PokemonSkill> PokemonSkills { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<SkillCondition> SkillConditions { get; set; }
        [InverseProperty("Skill")]
        public virtual ICollection<SkillGameVersion> SkillGameVersions { get; set; }
    }
}
