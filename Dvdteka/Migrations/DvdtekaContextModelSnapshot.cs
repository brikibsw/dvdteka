﻿// <auto-generated />
using System;
using Dvdteka.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Dvdteka.Migrations
{
    [DbContext(typeof(DvdtekaContext))]
    partial class DvdtekaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity("Dvdteka.Data.Director", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Directors");
                });

            modelBuilder.Entity("Dvdteka.Data.Dvd", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AvailableQuantity");

                    b.Property<int>("DirectorId");

                    b.Property<int>("GenreId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Price");

                    b.Property<int>("Quantity");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.HasIndex("DirectorId");

                    b.HasIndex("GenreId");

                    b.ToTable("Dvds");
                });

            modelBuilder.Entity("Dvdteka.Data.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Dvdteka.Data.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MemberId");

                    b.Property<string>("MemberName");

                    b.Property<DateTime>("ReturnTime");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Dvdteka.Data.InvoiceItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DvdId");

                    b.Property<string>("DvdName");

                    b.Property<int>("InvoiceId");

                    b.Property<decimal>("Price");

                    b.Property<DateTime>("RentTime");

                    b.Property<DateTime>("ReturnTime");

                    b.HasKey("Id");

                    b.HasIndex("DvdId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceItems");
                });

            modelBuilder.Entity("Dvdteka.Data.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Dvdteka.Data.MemberContact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MemberId");

                    b.Property<string>("Type")
                        .IsRequired();

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("MemberContacts");
                });

            modelBuilder.Entity("Dvdteka.Data.Rent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DvdId");

                    b.Property<int>("MemberId");

                    b.Property<decimal?>("Price");

                    b.Property<DateTime>("RentTime");

                    b.Property<DateTime?>("ReturnTime");

                    b.HasKey("Id");

                    b.HasIndex("DvdId");

                    b.HasIndex("MemberId");

                    b.ToTable("Rents");
                });

            modelBuilder.Entity("Dvdteka.Data.Dvd", b =>
                {
                    b.HasOne("Dvdteka.Data.Director", "Director")
                        .WithMany("Dvds")
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Dvdteka.Data.Genre", "Genre")
                        .WithMany("Dvds")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Dvdteka.Data.Invoice", b =>
                {
                    b.HasOne("Dvdteka.Data.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Dvdteka.Data.InvoiceItem", b =>
                {
                    b.HasOne("Dvdteka.Data.Dvd", "Dvd")
                        .WithMany()
                        .HasForeignKey("DvdId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Dvdteka.Data.Invoice", "Invoice")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Dvdteka.Data.MemberContact", b =>
                {
                    b.HasOne("Dvdteka.Data.Member", "Member")
                        .WithMany("MemberContacts")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Dvdteka.Data.Rent", b =>
                {
                    b.HasOne("Dvdteka.Data.Dvd", "Dvd")
                        .WithMany("Rents")
                        .HasForeignKey("DvdId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Dvdteka.Data.Member", "Member")
                        .WithMany("Rents")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
