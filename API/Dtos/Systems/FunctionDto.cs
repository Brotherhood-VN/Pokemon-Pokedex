namespace API.Dtos.Systems
{
    public class FunctionViewDto
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Title { get; set; }
        public List<ActionDetail> ActionDetails { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class ActionDetail
    {
        public long Id { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public bool IsShow { get; set; }
        public bool? IsMenu { get; set; }
        public int? Seq { get; set; }
        public bool? IsDelete { get; set; }
    }

    public class FunctionDto
    {
        public long Id { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsShow { get; set; }
        public bool? IsMenu { get; set; }
        public int? Seq { get; set; }
        public bool? IsDelete { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    public class FunctionParam
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }

    public class FunctionSearchParam
    {
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Keyword { get; set; }
    }
}