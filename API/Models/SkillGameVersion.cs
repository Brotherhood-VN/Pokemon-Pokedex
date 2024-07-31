using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("SkillGameVersion")]
    public partial class SkillGameVersion
    {
        [Key]
        public long Id { get; set; }
        public long SkillId { get; set; }
        public long GameVersionId { get; set; }
        [ForeignKey("SkillId")]
        [InverseProperty("SkillGameVersions")]
        public virtual Skill Skill { get; set; }
        [ForeignKey("GameVersionId")]
        [InverseProperty("SkillGameVersions")]
        public virtual GameVersion GameVersion { get; set; }
    }
}
