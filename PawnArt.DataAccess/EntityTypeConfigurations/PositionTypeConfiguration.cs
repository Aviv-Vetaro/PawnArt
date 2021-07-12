
using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PawnArt.Logic.BoardArrangment;

namespace PawnArt.DataAccess.EntityTypeConfigurations
{
    public class PositionTypeConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");
            builder.Property<Guid>("BoardId");
            builder.Property<Guid?>("CurrentPositionId");
            builder.HasOne<Board>().WithMany("previosPositions").HasForeignKey("BoardId");
            builder.HasOne<Board>().WithOne("currentPosition").HasForeignKey<Position>("CurrentPositionId").IsRequired(false);
            builder.HasMany<Square>("squares").WithOne();
            builder.HasOne<Square>("enPassentTakablePawnOfWhite").WithOne().IsRequired(false).HasForeignKey<Position>("EnPassentTakableWhitePawnId").IsRequired(false);
            builder.HasOne<Square>("enPassentTakablePawnOfBlack").WithOne().IsRequired(false).HasForeignKey<Position>("EnPassentTakableBlackPawnId").IsRequired(false);
            builder.Property("canWhiteCastleKingSide");
            builder.Property("canWhiteCastleQueenSide");
            builder.Property("canBlackCastleKingSide");
            builder.Property("canBlackCastleQueenSide");
            builder.Property("_50MovesRuleCounter");
            builder.Property<Guid>("Id").ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.Property<Guid?>("EnPassentTakableWhitePawnId");
            builder.Property<Guid?>("EnPassentTakableBlackPawnId");
        }
    }
}
/*
builder.ToTable("Positions");
builder.Property<Guid>("BoardId");

builder.HasOne<Board>().WithMany("previosPositions").HasForeignKey("BoardId");
builder.HasOne<Board>().WithOne("currentPosition").HasForeignKey<Position>("CurrentPositionId");
//-->
builder.HasMany<Square>("squares").WithOne().HasForeignKey("PositionId");


builder.HasOne(p => p.EnPassentTakablePawnOfWhite).WithOne().HasForeignKey<Position>("EnPassentTakableWhitePawnId");
builder.HasOne(p => p.EnPassentTakablePawnOfBlack).WithOne().HasForeignKey<Position>("EnPassentTakableBlackPawnId");
builder.Property<Guid>("EnPassentTakableWhitePawnId");
builder.Property<Guid>("EnPassentTakableBlackPawnId");

builder.Property("canWhiteCastleKingSide");
builder.Property("canWhiteCastleQueenSide");
builder.Property("canBlackCastleKingSide");
builder.Property("canBlackCastleQueenSide");
builder.Property("_50MovesRuleCounter");

builder.Property<Guid>("Id").ValueGeneratedOnAdd();
builder.HasKey("Id");
*/

/*
builder.ToTable("Positions");

builder.Property<Guid>("BoardId");
builder.Property<Guid?>("CurrentPositionId");
builder.HasOne<Board>().WithMany("previosPositions").HasForeignKey("BoardId");
builder.HasOne<Board>().WithOne("currentPosition").HasForeignKey<Position>("CurrentPositionId");



builder.HasMany<Square>("squaresInPosition").WithOne().HasForeignKey("SquareOfPositionId").IsRequired();

builder.HasOne(p => p.EnPassentTakablePawnOfWhite).WithOne().HasForeignKey<Position>("EnPassentTakableWhitePawnId").IsRequired(false);
builder.HasOne(p => p.EnPassentTakablePawnOfBlack).WithOne().HasForeignKey<Position>("EnPassentTakableBlackPawnId").IsRequired(false);

builder.Property("canWhiteCastleKingSide");
builder.Property("canWhiteCastleQueenSide");
builder.Property("canBlackCastleKingSide");
builder.Property("canBlackCastleQueenSide");
builder.Property("_50MovesRuleCounter");

builder.Property<Guid>("Id").ValueGeneratedOnAdd();
builder.HasKey("Id");

builder.Property<Guid?>("EnPassentTakableWhitePawnId");
builder.Property<Guid?>("EnPassentTakableBlackPawnId");
*/