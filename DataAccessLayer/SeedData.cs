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

        
        
    }
}
