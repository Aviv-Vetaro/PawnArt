// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PawnArt.DataAccess;

namespace PawnArt.DataAccess.Migrations
{
    [DbContext(typeof(GameContext))]
    partial class GameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PawnArt.Logic.BoardArrangment.Board", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CurrentPositionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("PawnArt.Logic.BoardArrangment.Piece", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Color")
                        .HasColumnType("int");

                    b.Property<bool>("Moved")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Pieces");
                });

            modelBuilder.Entity("PawnArt.Logic.BoardArrangment.Position", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CurrentPositionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EnPassentTakableBlackPawnId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("EnPassentTakableWhitePawnId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PlayerToPlayColor")
                        .HasColumnType("int");

                    b.Property<int>("_50MovesRuleCounter")
                        .HasColumnType("int");

                    b.Property<bool>("canBlackCastleKingSide")
                        .HasColumnType("bit");

                    b.Property<bool>("canBlackCastleQueenSide")
                        .HasColumnType("bit");

                    b.Property<bool>("canWhiteCastleKingSide")
                        .HasColumnType("bit");

                    b.Property<bool>("canWhiteCastleQueenSide")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.HasIndex("CurrentPositionId")
                        .IsUnique()
                        .HasFilter("[CurrentPositionId] IS NOT NULL");

                    b.HasIndex("EnPassentTakableBlackPawnId")
                        .IsUnique()
                        .HasFilter("[EnPassentTakableBlackPawnId] IS NOT NULL");

                    b.HasIndex("EnPassentTakableWhitePawnId")
                        .IsUnique()
                        .HasFilter("[EnPassentTakableWhitePawnId] IS NOT NULL");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("PawnArt.Logic.BoardArrangment.Square", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("File")
                        .HasColumnType("int");

                    b.Property<Guid?>("OccupierId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PositionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Row")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OccupierId")
                        .IsUnique()
                        .HasFilter("[OccupierId] IS NOT NULL");

                    b.HasIndex("PositionId");

                    b.ToTable("Sqaures");
                });

            modelBuilder.Entity("PawnArt.Logic.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BlackPlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Recurring")
                        .HasColumnType("bit");

                    b.Property<Guid>("WhitePlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BlackPlayerId");

                    b.HasIndex("WhitePlayerId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("PawnArt.Logic.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("PawnArt.Logic.BoardArrangment.Board", b =>
                {
                    b.HasOne("PawnArt.Logic.Game", null)
                        .WithOne("Board")
                        .HasForeignKey("PawnArt.Logic.BoardArrangment.Board", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PawnArt.Logic.BoardArrangment.Position", b =>
                {
                    b.HasOne("PawnArt.Logic.BoardArrangment.Board", null)
                        .WithMany("previosPositions")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PawnArt.Logic.BoardArrangment.Board", null)
                        .WithOne("currentPosition")
                        .HasForeignKey("PawnArt.Logic.BoardArrangment.Position", "CurrentPositionId");

                    b.HasOne("PawnArt.Logic.BoardArrangment.Square", "enPassentTakablePawnOfBlack")
                        .WithOne()
                        .HasForeignKey("PawnArt.Logic.BoardArrangment.Position", "EnPassentTakableBlackPawnId");

                    b.HasOne("PawnArt.Logic.BoardArrangment.Square", "enPassentTakablePawnOfWhite")
                        .WithOne()
                        .HasForeignKey("PawnArt.Logic.BoardArrangment.Position", "EnPassentTakableWhitePawnId");

                    b.Navigation("enPassentTakablePawnOfBlack");

                    b.Navigation("enPassentTakablePawnOfWhite");
                });

            modelBuilder.Entity("PawnArt.Logic.BoardArrangment.Square", b =>
                {
                    b.HasOne("PawnArt.Logic.BoardArrangment.Piece", "Occupier")
                        .WithOne()
                        .HasForeignKey("PawnArt.Logic.BoardArrangment.Square", "OccupierId");

                    b.HasOne("PawnArt.Logic.BoardArrangment.Position", null)
                        .WithMany("squares")
                        .HasForeignKey("PositionId");

                    b.Navigation("Occupier");
                });

            modelBuilder.Entity("PawnArt.Logic.Game", b =>
                {
                    b.HasOne("PawnArt.Logic.Player", "BlackPlayer")
                        .WithMany()
                        .HasForeignKey("BlackPlayerId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("PawnArt.Logic.Player", "WhitePlayer")
                        .WithMany()
                        .HasForeignKey("WhitePlayerId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.OwnsOne("PawnArt.Logic.ClockState", "state", b1 =>
                        {
                            b1.Property<Guid>("GameId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("blackPlayerTime")
                                .IsRequired()
                                .HasColumnType("nvarchar(48)");

                            b1.Property<string>("lastPlyTime")
                                .IsRequired()
                                .HasColumnType("nvarchar(48)");

                            b1.Property<string>("whitePlayerTime")
                                .IsRequired()
                                .HasColumnType("nvarchar(48)");

                            b1.HasKey("GameId");

                            b1.ToTable("Games");

                            b1.WithOwner()
                                .HasForeignKey("GameId");
                        });

                    b.Navigation("BlackPlayer");

                    b.Navigation("state");

                    b.Navigation("WhitePlayer");
                });

            modelBuilder.Entity("PawnArt.Logic.BoardArrangment.Board", b =>
                {
                    b.Navigation("currentPosition")
                        .IsRequired();

                    b.Navigation("previosPositions");
                });

            modelBuilder.Entity("PawnArt.Logic.BoardArrangment.Position", b =>
                {
                    b.Navigation("squares");
                });

            modelBuilder.Entity("PawnArt.Logic.Game", b =>
                {
                    b.Navigation("Board")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
