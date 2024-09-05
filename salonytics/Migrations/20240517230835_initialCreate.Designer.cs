﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using salonytics.Data;

#nullable disable

namespace salonytics.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240517230835_initialCreate")]
    partial class initialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Salonytics.Models.Entities.Appointment", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BranchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ContactNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<int>("NoOfHeads")
                        .HasColumnType("int");

                    b.Property<string>("Remarks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("StylistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("TotalPrice")
                        .HasColumnType("real");

                    b.HasKey("AppointmentId");

                    b.HasIndex("BranchId");

                    b.HasIndex("ServiceId");

                    b.HasIndex("StylistId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("Salonytics.Models.Entities.Rating", b =>
                {
                    b.Property<Guid>("RatingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<Guid>("StylistId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RatingId");

                    b.HasIndex("StylistId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("Salonytics.Models.Entities.Service", b =>
                {
                    b.Property<Guid>("ServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<Guid?>("StylistId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ServiceId");

                    b.HasIndex("StylistId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Salonytics.Models.Entities.Stylist", b =>
                {
                    b.Property<Guid>("StylistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BranchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("StylistId");

                    b.HasIndex("BranchId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Stylists");
                });

            modelBuilder.Entity("salonytics.Models.Entities.Branch", b =>
                {
                    b.Property<Guid>("BranchId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BranchName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BranchId");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("salonytics.Models.Entities.Customer", b =>
                {
                    b.Property<string>("CustomerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("salonytics.Models.Entities.Manager", b =>
                {
                    b.Property<Guid>("ManagerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BranchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ManagerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ManagerId");

                    b.HasIndex("BranchId");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("salonytics.Models.Entities.StylistSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("EndTime")
                        .HasColumnType("time");

                    b.Property<bool>("IsRestDay")
                        .HasColumnType("bit");

                    b.Property<bool>("IsWorking")
                        .HasColumnType("bit");

                    b.Property<TimeSpan?>("StartTime")
                        .HasColumnType("time");

                    b.Property<Guid>("StylistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("WeekStartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("StylistId");

                    b.ToTable("StylistSchedules");
                });

            modelBuilder.Entity("Salonytics.Models.Entities.Appointment", b =>
                {
                    b.HasOne("salonytics.Models.Entities.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Salonytics.Models.Entities.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Salonytics.Models.Entities.Stylist", "Stylist")
                        .WithMany()
                        .HasForeignKey("StylistId");

                    b.Navigation("Branch");

                    b.Navigation("Service");

                    b.Navigation("Stylist");
                });

            modelBuilder.Entity("Salonytics.Models.Entities.Rating", b =>
                {
                    b.HasOne("Salonytics.Models.Entities.Stylist", "Stylist")
                        .WithMany("Ratings")
                        .HasForeignKey("StylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stylist");
                });

            modelBuilder.Entity("Salonytics.Models.Entities.Service", b =>
                {
                    b.HasOne("Salonytics.Models.Entities.Stylist", null)
                        .WithMany("Services")
                        .HasForeignKey("StylistId");
                });

            modelBuilder.Entity("Salonytics.Models.Entities.Stylist", b =>
                {
                    b.HasOne("salonytics.Models.Entities.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("salonytics.Models.Entities.Manager", null)
                        .WithMany("Stylists")
                        .HasForeignKey("ManagerId");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("salonytics.Models.Entities.Manager", b =>
                {
                    b.HasOne("salonytics.Models.Entities.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("salonytics.Models.Entities.StylistSchedule", b =>
                {
                    b.HasOne("Salonytics.Models.Entities.Stylist", null)
                        .WithMany("Schedules")
                        .HasForeignKey("StylistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Salonytics.Models.Entities.Stylist", b =>
                {
                    b.Navigation("Ratings");

                    b.Navigation("Schedules");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("salonytics.Models.Entities.Manager", b =>
                {
                    b.Navigation("Stylists");
                });
#pragma warning restore 612, 618
        }
    }
}
