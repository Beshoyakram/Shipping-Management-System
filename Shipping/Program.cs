using Shipping.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Shipping.Filters;
using Shipping.Repository.ArabicNamesColumnIntoRoleClaimsTable;
using Shipping.Repository;

namespace Shipping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<MyContext>(o =>
            o.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();


            builder.Services.AddScoped<IAddArabicNamesToRoleCaimsTable, AddArabicNamesToRoleCaimsTable>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IMerchantRepository, MerchantRepository>();
            builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();


            //For igonring reload page when change permissions
            builder.Services.Configure<SecurityStampValidatorOptions>(
                options =>
                {
                    options.ValidationInterval = TimeSpan.Zero;
                }) ;


            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
                option =>
                {
                    option.Password.RequireUppercase = false;
                }
                ).AddEntityFrameworkStores<MyContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            /*app.UseAuthentication();*/   
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}