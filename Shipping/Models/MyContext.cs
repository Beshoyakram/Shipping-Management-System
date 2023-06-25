using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Shipping.Models
{
    public class MyContext:IdentityDbContext<ApplicationUser>
    {

        public MyContext():base()
        {}
        public MyContext(DbContextOptions<MyContext> options)
        : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Seeding Data For ApplicationUser table with Role Admin
            string ADMIN_ID = Guid.NewGuid().ToString();
            string ROLE_ID = Guid.NewGuid().ToString();

            ApplicationUser admin = new ApplicationUser
            {
                Id = ADMIN_ID,
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                EmailConfirmed = true,
                UserName = "admin",
                NormalizedUserName = "admin@gmail.com".ToUpper(),
                Address = "Cairo",
                LockoutEnabled = true,
                ConcurrencyStamp = DateTime.Now.ToString()

            };
            var hasher = new PasswordHasher<ApplicationUser>();
            admin.PasswordHash = hasher.HashPassword(admin, password: "@Admin123");
            modelBuilder.Entity<ApplicationUser>().HasData(admin);


            //Admin Role
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ROLE_ID,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    ConcurrencyStamp = DateTime.Now.ToString()
                });
            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole
               {
                   Name = "User",
                   NormalizedName = "User".ToUpper(),
                   ConcurrencyStamp = DateTime.Now.ToString()
               });
            //Connect An admin to Role Admin
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                { RoleId = ROLE_ID, UserId = ADMIN_ID });

            //Add Permissions
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(
                new IdentityRoleClaim<string>
                {
                    Id = 1,
                    ClaimType = "Permissions",
                    ClaimValue = "Permissions.Controls.View",
                    RoleId = ROLE_ID
                },
                new IdentityRoleClaim<string>
                {
                    Id = 2,
                    ClaimType = "Permissions",
                    ClaimValue = "Permissions.Controls.Edit",
                    RoleId = ROLE_ID
                },
                new IdentityRoleClaim<string>
                {
                    Id = 3,
                    ClaimType = "Permissions",
                    ClaimValue = "Permissions.Controls.Delete",
                    RoleId = ROLE_ID
                },
                new IdentityRoleClaim<string>
                {
                    Id = 4,
                    ClaimType = "Permissions",
                    ClaimValue = "Permissions.Controls.Create",
                    RoleId = ROLE_ID
                }
                );


            #endregion

        }
    }
}
