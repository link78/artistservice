using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
   public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public object Users { get; internal set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Artist>().ToTable("Artists").HasMany(a => a.Albums)
                .WithOne(a => a.Artist);
            builder.Entity<Album>().ToTable("Albums").HasOne(a => a.Artist)
                .WithMany(a => a.Albums);

            base.OnModelCreating(builder);
        }
    }
}
