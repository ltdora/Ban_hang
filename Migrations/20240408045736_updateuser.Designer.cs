﻿// <auto-generated />
using System;
using He_thong_ban_hang;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace He_thong_ban_hang.Migrations
{
    [DbContext(typeof(ShopContext))]
    [Migration("20240408045736_updateuser")]
    partial class updateuser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("He_thong_ban_hang.Employees", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmployeeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeePassword")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("He_thong_ban_hang.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int?>("UsersUserId")
                        .HasColumnType("int");

                    b.HasKey("OrderID");

                    b.HasIndex("UsersUserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("He_thong_ban_hang.OrderDetail", b =>
                {
                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("ProductsProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ProductID", "OrderID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductsProductID");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("He_thong_ban_hang.Products", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("ProductPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ProductID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("He_thong_ban_hang.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserPassword")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Uses");
                });

            modelBuilder.Entity("He_thong_ban_hang.Order", b =>
                {
                    b.HasOne("He_thong_ban_hang.Users", null)
                        .WithMany("order")
                        .HasForeignKey("UsersUserId");
                });

            modelBuilder.Entity("He_thong_ban_hang.OrderDetail", b =>
                {
                    b.HasOne("He_thong_ban_hang.Order", null)
                        .WithMany("orderDetail")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("He_thong_ban_hang.Products", null)
                        .WithMany("orderDetails")
                        .HasForeignKey("ProductsProductID");
                });

            modelBuilder.Entity("He_thong_ban_hang.Order", b =>
                {
                    b.Navigation("orderDetail");
                });

            modelBuilder.Entity("He_thong_ban_hang.Products", b =>
                {
                    b.Navigation("orderDetails");
                });

            modelBuilder.Entity("He_thong_ban_hang.Users", b =>
                {
                    b.Navigation("order");
                });
#pragma warning restore 612, 618
        }
    }
}
