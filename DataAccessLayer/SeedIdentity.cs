using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;

namespace DataAccessLayer
{
   public static class SeedIdentity
    {
        public async static void SeedUser(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<IdentityContext>();
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            var roleManger = provider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();


            if (!context.Users.Any())
            {

               

                var admin = new AppUser
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "Admin",
                    Email = "adminKade@me.com",
                    FirstName = "Adams",
                    IsSuperUser = true,
                    LastName = "Rice",
                    CreatedAt = DateTime.Now
                };

                




                if (!await roleManger.RoleExistsAsync("Administrator"))
                {
                    var role = new IdentityRole("Administrator");
                    await roleManger.CreateAsync(role);
                    await roleManger.AddClaimAsync(role, new Claim("IsAdmin","G.M"));
                    await roleManger.AddClaimAsync(role, new Claim("IsSuper", "SuperUser"));

                }
                if (await userManager.FindByNameAsync(admin.UserName) == null)
                {
                    await userManager.CreateAsync(admin, "Pass4Admin$$");
                    await userManager.AddToRoleAsync(admin, "Administrator");
                    admin.EmailConfirmed = true;
                    admin.LockoutEnabled = false;
                }

                // adding second user

                var user = new AppUser
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "Admin",
                    Email = "Aline@me.com",
                    FirstName = "Alice",
                    IsSuperUser = false,
                    LastName = "Fry",
                    CreatedAt = DateTime.Now
                };

                if (!await roleManger.RoleExistsAsync("RegisteredUser"))
                {
                    var role = new IdentityRole("RegisteredUser");
                    await roleManger.CreateAsync(role);
                    await roleManger.AddClaimAsync(role, new Claim("IsAdmin", "Admin_Sec"));

                }
                if (await userManager.FindByNameAsync(user.UserName) == null)
                {
                    await userManager.CreateAsync(user, "Pass4Aline$$");
                    await userManager.AddToRoleAsync(user, "RegisteredUser");
                    user.EmailConfirmed = true;
                    user.LockoutEnabled = false;
                }
            }
        }

    }
}
