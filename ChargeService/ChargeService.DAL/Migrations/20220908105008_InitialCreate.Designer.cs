﻿// <auto-generated />
using System;
using ChargeService.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChargeService.DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220908105008_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence<int>("FillingNumbers");

            modelBuilder.HasSequence<int>("SessionNumbers");

            modelBuilder.Entity("ChargeService.DAL.Entities.Filling", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('\"FillingNumbers\"')");

                    b.Property<decimal>("BonusAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("BonusCalculateRuleId")
                        .HasColumnType("integer");

                    b.Property<decimal>("PromotionAmount")
                        .HasColumnType("numeric");

                    b.Property<int>("PromotionId")
                        .HasColumnType("integer");

                    b.Property<int>("PumpId")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalMoneyAmount")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Fillings");
                });

            modelBuilder.Entity("ChargeService.DAL.Entities.Session", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValueSql("nextval('\"SessionNumbers\"')");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("FillingId")
                        .HasColumnType("bigint");

                    b.Property<int>("Minutes")
                        .HasColumnType("integer");

                    b.Property<Guid>("RequestId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("ChargeService.DAL.Entities.Session", b =>
                {
                    b.HasOne("ChargeService.DAL.Entities.Filling", "Filling")
                        .WithOne("Session")
                        .HasForeignKey("ChargeService.DAL.Entities.Session", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Filling");
                });

            modelBuilder.Entity("ChargeService.DAL.Entities.Filling", b =>
                {
                    b.Navigation("Session")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
