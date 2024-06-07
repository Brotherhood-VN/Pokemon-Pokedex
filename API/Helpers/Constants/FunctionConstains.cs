namespace API.Helpers.Constants
{
    public static class FunctionConstains
    {
        public static readonly List<string> Menus = new()
        {
            // Systems
            "Account", "AccountType",
            "Classification", "Condition",
            "Function",
            "Gender",
            "Menu",
            "Role",
            "Skill", "Stone"
        };

        public static readonly List<string> Actions = new()
        {
            "Create",
            "Update",
            "Delete",
            "GetDataPagination",
            "GetDetail",
            "LoadMenus",
            "ConfigurationMenus"
        };
    }
}