﻿namespace Shipping.Constants
{
    public enum Modules
    {
        Controls,
        Merchants,
        Deliveries,
        Employees,
        Orders,
        Branches,
        Staties,
        Cities

    }
    public static class EnglishVsArabic
    {
        public static readonly Dictionary<string, string> ModulesInEn_AR = new Dictionary<string, string>
        {
            { "Controls", "صلاحيات المجموعات" },
            { "Merchants", "صلاحيات التجار" },
            { "Deliveries", "صلاحيات المناديب" },
            { "Employees", "صلاحيات الموظفين" },
            { "Orders", "صلاحيات الطلبات" },
            { "Branches", "صلاحيات الافرع" },
            { "Staties", "صلاحيات المحافظات" },
            { "Cities", "صلاحيات المدن" }


        };
    }
    


}
