﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201227170307_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Domain.DefaultBoat", b =>
                {
                    b.Property<int>("DefaultBoatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("DefaultBoatId");

                    b.ToTable("DefaultBoats");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("BoardTurnId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("GameOptionId")
                        .HasColumnType("int");

                    b.Property<bool>("GameOver")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GameId");

                    b.HasIndex("GameOptionId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Domain.GameBoard", b =>
                {
                    b.Property<int>("GameBoardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("AttackerId")
                        .HasColumnType("int");

                    b.Property<string>("BoardJson")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DefenderId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("GameBoardId");

                    b.HasIndex("AttackerId")
                        .IsUnique()
                        .HasFilter("[AttackerId] IS NOT NULL");

                    b.HasIndex("DefenderId")
                        .IsUnique()
                        .HasFilter("[DefenderId] IS NOT NULL");

                    b.HasIndex("GameId");

                    b.ToTable("GameBoards");
                });

            modelBuilder.Entity("Domain.GameBoat", b =>
                {
                    b.Property<int>("GameBoatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FacingX")
                        .HasColumnType("int");

                    b.Property<int?>("FacingY")
                        .HasColumnType("int");

                    b.Property<int>("GameBoardId")
                        .HasColumnType("int");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<int?>("LocationX")
                        .HasColumnType("int");

                    b.Property<int?>("LocationY")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GameBoatId");

                    b.HasIndex("GameBoardId");

                    b.ToTable("GameBoats");
                });

            modelBuilder.Entity("Domain.GameOption", b =>
                {
                    b.Property<int>("GameOptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("BoardHeight")
                        .HasColumnType("int");

                    b.Property<int>("BoardWidth")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("EBoatsCanTouch")
                        .HasColumnType("int");

                    b.Property<int>("ENextMoveAfterHit")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("GameOptionId");

                    b.ToTable("GameOptions");
                });

            modelBuilder.Entity("Domain.GameOptionBoat", b =>
                {
                    b.Property<int>("GameOptionBoatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DefaultBoatId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameOptionId")
                        .HasColumnType("int");

                    b.HasKey("GameOptionBoatId");

                    b.HasIndex("DefaultBoatId");

                    b.HasIndex("GameOptionId");

                    b.ToTable("GameOptionBoats");
                });

            modelBuilder.Entity("Domain.TurnSave", b =>
                {
                    b.Property<int>("TurnSaveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AttackerId")
                        .HasColumnType("int");

                    b.Property<int>("CellX")
                        .HasColumnType("int");

                    b.Property<int>("CellY")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DefenderId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeletedDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.HasKey("TurnSaveId");

                    b.HasIndex("GameId");

                    b.ToTable("TurnSaves");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.HasOne("Domain.GameOption", "GameOption")
                        .WithMany("Games")
                        .HasForeignKey("GameOptionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("GameOption");
                });

            modelBuilder.Entity("Domain.GameBoard", b =>
                {
                    b.HasOne("Domain.TurnSave", "Attacker")
                        .WithOne("Attacker")
                        .HasForeignKey("Domain.GameBoard", "AttackerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Domain.TurnSave", "Defender")
                        .WithOne("Defender")
                        .HasForeignKey("Domain.GameBoard", "DefenderId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Domain.Game", "Game")
                        .WithMany("GameBoards")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attacker");

                    b.Navigation("Defender");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Domain.GameBoat", b =>
                {
                    b.HasOne("Domain.GameBoard", "GameBoard")
                        .WithMany("GameBoats")
                        .HasForeignKey("GameBoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameBoard");
                });

            modelBuilder.Entity("Domain.GameOptionBoat", b =>
                {
                    b.HasOne("Domain.DefaultBoat", "DefaultBoat")
                        .WithMany("GameOptionBoats")
                        .HasForeignKey("DefaultBoatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.GameOption", "GameOption")
                        .WithMany("GameOptionBoats")
                        .HasForeignKey("GameOptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DefaultBoat");

                    b.Navigation("GameOption");
                });

            modelBuilder.Entity("Domain.TurnSave", b =>
                {
                    b.HasOne("Domain.Game", "Game")
                        .WithMany("TurnSaves")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Domain.DefaultBoat", b =>
                {
                    b.Navigation("GameOptionBoats");
                });

            modelBuilder.Entity("Domain.Game", b =>
                {
                    b.Navigation("GameBoards");

                    b.Navigation("TurnSaves");
                });

            modelBuilder.Entity("Domain.GameBoard", b =>
                {
                    b.Navigation("GameBoats");
                });

            modelBuilder.Entity("Domain.GameOption", b =>
                {
                    b.Navigation("GameOptionBoats");

                    b.Navigation("Games");
                });

            modelBuilder.Entity("Domain.TurnSave", b =>
                {
                    b.Navigation("Attacker")
                        .IsRequired();

                    b.Navigation("Defender")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
