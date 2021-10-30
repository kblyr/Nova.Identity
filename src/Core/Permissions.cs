using System.Security.Cryptography;
namespace Nova.Identity
{
    public static class Permissions
    {
        public static class Boundary
        {
            public const string Add = "IDENTITY_BOUNDARY_ADD";
            public const string Edit = "IDENTITY_BOUNDARY_EDIT";
            public const string Delete = "IDENTITY_BOUNDARY_DELETE";
        }

        public static class ClientApp
        {
            public const string Add = "IDENTITY_CLIENTAPP_ADD";
            public const string Edit = "IDENTITY_CLIENTAPP_EDIT";
            public const string Delete = "IDENTITY_CLIENTAPP_DELETE";
        }

        public static class Role
        {
            public const string Add = "IDENTITY_ROLE_ADD";
            public const string Edit = "IDENTITY_ROLE_EDIT";
            public const string Delete = "IDENTITY_ROLE_DELETE";
        }

        public static class Permission
        {
            public const string Add = "IDENTITY_PERMISSION_ADD";
            public const string Edit = "IDENTITY_PERMISSION_EDIT";
            public const string Delete = "IDENTITY_PERMISSION_DELETE";
        }

        public static class RolePermission
        {
            public const string Add = "IDENTITY_ROLEPERMISSION_ADD";
            public const string Delete = "IDENTITY_ROLEPERMISSION_DELETE";
        }
    }
}