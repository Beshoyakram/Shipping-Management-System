namespace Shipping.Constants
{
    public enum Modules
    {
        Controls,
        Merchants,
        Representative,
        Employees

    }
    public static class EnglishVsArabic
    {
        public static readonly Dictionary<string, string> ModulesInEn_AR = new Dictionary<string, string>
        {
            { "Controls", "الصلاحيات" },
            { "Merchants", "التجار" },
            { "Representative", "المناديب" },
            { "Employees", "الموظفين" }

        };
    }
    


}
