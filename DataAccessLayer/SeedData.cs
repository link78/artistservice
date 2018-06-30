using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
   public static class SeedData
    {
        public static void Seed(this DataContext context)
        {
            if (context.Artists.Any())
            {
                return;
            }

            var artistList = new List<Artist>
            {
                new Artist
                {
                    FullName="Jeans Wilikoz", ArtistName="Jeanny Sunny",
                    Gender= "Femal",
                    Contact = new Contact
                    {
                        Email="jeanny@me.com",
                        Phone="514-895-7885",
                        Tweeter="@jeanny"
                    },
                    Manager = new Manager
                    {
                        FullName ="Willis Doe",
                        Email ="willi@me.com",
                        Tweeter="@doe75",
                        Phone="321-452-0025"
                    },
                    Albums = new List<Album>
                    {
                        new Album {Name="Waking-Up", Genre="Classic", Price=9.99m, Producer="Mot-Town", ReleasedDate=DateTime.Parse("5/9/2017")}
                    }
                    
                },

                new Artist
                {
                    FullName="Ed Sheeran", ArtistName="Ed",
                    Gender= "Male",
                    Contact = new Contact
                    {
                        Email="edsheeran@me.com",
                        Phone="724-895-7885",
                        Tweeter="@sheeran"
                    },
                    Manager = new Manager
                    {
                        FullName ="Lopez Gloria",
                        Email ="lopezg@me.com",
                        Tweeter="@lopezG",
                        Phone="485-452-0025"
                    },
                    Albums = new List<Album>
                    {
                        new Album {Name="Divide", Genre="Alternative Pop/Rock", Price=14.99m, Producer="Platium", ReleasedDate=DateTime.Parse("2/19/2015")},
                         new Album {Name="Divide", Genre="Alternative Pop/Rock", Price=14.99m, Producer="Platium", ReleasedDate=DateTime.Parse("3/15/2018")}
                    }

                }

    
            };

            context.AddRange(artistList);
            context.SaveChanges();
        }

        public async static void SeedUser(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<DataContext>();
            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            var roleManger = provider.GetRequiredService<RoleManager<IdentityRole>>();


            if (context.Users.Any())
            {
                var admin = new AppUser
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "Admin",
                    Email = "adminKade@me.com",
                    FirstName = "Adams",
                    IsSuperUser= true,
                    LastName = "Rice",
                    CreatedAt = DateTime.Now
                };

                if(!await roleManger.RoleExistsAsync("Administrator"))
                {
                    await roleManger.CreateAsync(new IdentityRole("Administrator"));
                }
                if(await userManager.FindByNameAsync(admin.UserName)== null)
                {
                    await userManager.CreateAsync(admin, "Pass4Admin$$");
                    await userManager.AddToRoleAsync(admin, "Administrator");
                    admin.EmailConfirmed = true;
                    admin.LockoutEnabled = false;
                }


                var user = new AppUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "Admin",
                    Email = "Aline@me.com",
                    FirstName = "Alice",
                    IsSuperUser = false,
                    LastName = "Fry",
                    CreatedAt = DateTime.Now
                };

                var userResult = await userManager.CreateAsync(user, "Pass4Aline$$");
                var roleResult = await userManager.AddToRoleAsync(user, "RegisteredUser");
                if(! userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to add user and role");
                }
            }
        }
    }
}
