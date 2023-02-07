﻿// <auto-generated />
using System;
using AdministrationSystem.Eamv.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AdministrationSystem.Eamv.Migrations.MainDb
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20221117110848_InitialActivities2")]
    partial class InitialActivities2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AdministrationSystem.Eamv.Models.Activity", b =>
                {
                    b.Property<int>("Activityid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Activityid"), 1L, 1);

                    b.Property<string>("ByWhom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.Property<int>("RoomID")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Activityid");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("RoomID");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("AdministrationSystem.Eamv.Models.Date", b =>
                {
                    b.Property<int>("DateID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DateID"), 1L, 1);

                    b.Property<int?>("Activityid")
                        .HasColumnType("int");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.HasKey("DateID");

                    b.HasIndex("Activityid");

                    b.ToTable("Date");
                });

            modelBuilder.Entity("AdministrationSystem.Eamv.Models.Department", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentID"), 1L, 1);

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("DepartmentID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("AdministrationSystem.Eamv.Models.Room", b =>
                {
                    b.Property<int>("RoomID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomID"), 1L, 1);

                    b.Property<int>("DepartmentID")
                        .HasColumnType("int");

                    b.Property<string>("RoomName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("RoomID");

                    b.HasIndex("DepartmentID");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("AdministrationSystem.Eamv.Models.Activity", b =>
                {
                    b.HasOne("AdministrationSystem.Eamv.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AdministrationSystem.Eamv.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("AdministrationSystem.Eamv.Models.Date", b =>
                {
                    b.HasOne("AdministrationSystem.Eamv.Models.Activity", null)
                        .WithMany("Dates")
                        .HasForeignKey("Activityid");
                });

            modelBuilder.Entity("AdministrationSystem.Eamv.Models.Room", b =>
                {
                    b.HasOne("AdministrationSystem.Eamv.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("AdministrationSystem.Eamv.Models.Activity", b =>
                {
                    b.Navigation("Dates");
                });
#pragma warning restore 612, 618
        }
    }
}
