namespace Nova.Identity
{
    static class DatabaseInfo
    {
        public const string Schema = "Identity";

        public static class Tables
        {
            public const string Boundary = "Boundary";
            public const string ClientApp = "ClientApp";
            public const string Permission = "Permission";
            public const string Role = "Role";
            public const string RolePermission = "RolePermission";
            public const string User = "User";
            public const string UserClientApp = "UserClientApp";
            public const string UserRole = "UserRole";
        }
    }
}