namespace API.Helpers.Attributes
{
    /// <summary>
    ///     Kiểm tra Action để cấu hình phân quyền.
    /// </summary>
    /// <params>
    ///   seq:
    ///    Sắp xếp thứ tự Action.
    /// </params>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    sealed class MenuMemberAttribute : Attribute
    {
        private readonly int _seq;

        public MenuMemberAttribute(int seq)
        {
            _seq = seq;
        }

        public int Seq
        {
            get { return _seq; }
        }
    }
}