namespace API.Dtos.Systems
{
    public class AbilityDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Effect { get; set; }
        public string InDepthEffect { get; set; }
        public bool Status { get; set; }
        public bool IsDelete { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }

    }
}