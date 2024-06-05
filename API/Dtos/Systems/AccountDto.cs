namespace API.Dtos.Systems
{
    public class AccountDto
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }
        public bool? IsDelete { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public long AccountTypeId { get; set; }
        public string AccountTypeTitle { get; set; }

        public List<long> RoleIds { get; set; }
        public List<long> FunctionIds { get; set; }
    }

    public class AccountChangePassword
    {
        public long Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public long? UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}