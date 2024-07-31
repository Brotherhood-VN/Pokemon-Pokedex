namespace API.Helpers.Attributes
{
    /// <summary>
    ///   Xác định Controller này có phải là menu hệ thống hay không.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class IsMenuAttribute : Attribute
    {

    }
}