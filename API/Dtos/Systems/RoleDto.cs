namespace API.Dtos.Systems
{
    public class RoleDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public bool? IsDelete { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }

        public List<long> FunctionIds { get; set; }
    }

    public class RoleAuth
    {
        public long Id { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public long? ParentId { get; set; }
        public bool? IsShow { get; set; }
        public bool IsActive { get; set; }
    }

    public class RoleUser
    {
        public long Id { get; set; }
        public string Controller { get; set; }
        public string Title { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
        public List<SubRoleUser> Actions { get; set; }
    }

    public class SubRoleUser
    {
        public long Id { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public long ParentId { get; set; }
        public int? Seq { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDisabled { get; set; }
    }

    public class RoleUserParams
    {
        public bool CheckedAll { get; set; }
        public List<long> RoleIds { get; set; } = new(); // List RoleId
        public long? AccountId { get; set; } // AccountId
    }
}