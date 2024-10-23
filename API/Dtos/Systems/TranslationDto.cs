namespace API.Dtos.Systems
{
    public class TranslationDto
    {
        public long Id { get; set; }
        public string FromTable { get; set; }
        public string Language { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool? Status { get; set; }
        public bool? IsDelete { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }

    }
}