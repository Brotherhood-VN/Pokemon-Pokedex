namespace API.Helpers.Constants
{
    public static class FunctionConstains
    {
        public static readonly List<string> Menus = new()
        {
            // Systems
            "About", "Account", "AccountType",
            "BookingStatus", "Branch",
            "Combo",
            "Dashboard", "Department", "District",
            "Function",
            "Gender",
            "InvoiceStatus", "InvoiceType",
            "MemberType", "Menu",
            "OrderStatus", "OrderType",
            "Position", "ProductCategory", "ProductType", "Province",
            "Role",
            "Service", "ServiceCategory", "ServiceType", "Staff", "StaffType",
            "TermAndPolicy",
            "Ward"
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