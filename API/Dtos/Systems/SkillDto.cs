namespace API.Dtos.Systems
{
    public class SkillDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Effect { get; set; }
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
        public string Classes { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }

        public List<SkillConditionDto> SkillConditions { get; set; }
        public List<SkillGameVersionDto> SkillGameVersions { get; set; }
    }
}