using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("SkillCondition")]
    public partial class SkillCondition
    {
        [Key]
        public long Id { get; set; }
        public long SkillId { get; set; }
        public long ConditionId { get; set; }
        [ForeignKey("SkillId")]
        [InverseProperty("SkillConditions")]
        public virtual Skill Skill { get; set; }
        [ForeignKey("ConditionId")]
        [InverseProperty("SkillConditions")]
        public virtual Condition Condition { get; set; }
    }
}
