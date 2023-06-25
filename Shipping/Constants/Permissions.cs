using NuGet.Packaging;

namespace Shipping.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionList(String module) {

            return new List<string>()
            {
                $"Permissions.{module}.View",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
         

        }
        public static List<string> GenerateAllPermissions()
        {

            var allPermissions = new List<string>();
            var modules = Enum.GetValues(typeof(Modules));
            foreach (var module in modules)
            {
                allPermissions.AddRange(GeneratePermissionList(module.ToString()));
            }

            return allPermissions;
        }
        public static class Controls
        {
            public const string View = "Permissions.Controls.View";
            public const string Create = "Permissions.Controls.Create";
            public const string Edit = "Permissions.Controls.Edit";
            public const string Delete = "Permissions.Controls.Delete";
        }
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }

    }
}
