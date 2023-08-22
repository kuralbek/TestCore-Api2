﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TestCore_Api.Context;

#nullable disable

namespace TestCore_Api.Migrations
{
    [DbContext(typeof(ApplicContext))]
    partial class ApplicContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TestCore_Api.ContactDir.Contact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ContactId"));

                    b.Property<string>("ContactName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContactValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("isMarked")
                        .HasColumnType("boolean");

                    b.HasKey("ContactId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("TestCore_Api.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("passwordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("passwordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
