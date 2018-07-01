﻿// <auto-generated />
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180701152345_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataAccessLayer.Entities.Album", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ArtistId");

                    b.Property<string>("Genre");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<string>("Producer");

                    b.Property<DateTime>("ReleasedDate");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("Albums");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Artist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ArtistName");

                    b.Property<Guid?>("ContactId");

                    b.Property<string>("FullName");

                    b.Property<string>("Gender");

                    b.Property<Guid?>("ManagerId");

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Phone");

                    b.Property<string>("Tweeter");

                    b.HasKey("Id");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Manager", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<string>("Phone");

                    b.Property<string>("Tweeter");

                    b.HasKey("Id");

                    b.ToTable("Manager");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Album", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Artist", "Artist")
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId");
                });

            modelBuilder.Entity("DataAccessLayer.Entities.Artist", b =>
                {
                    b.HasOne("DataAccessLayer.Entities.Contact", "Contact")
                        .WithMany()
                        .HasForeignKey("ContactId");

                    b.HasOne("DataAccessLayer.Entities.Manager", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");
                });
#pragma warning restore 612, 618
        }
    }
}