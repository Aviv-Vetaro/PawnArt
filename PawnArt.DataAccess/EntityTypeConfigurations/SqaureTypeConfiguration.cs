
using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PawnArt.Logic.BoardArrangment;

namespace PawnArt.DataAccess.EntityTypeConfigurations
{
    public class SqaureTypeConfiguration : IEntityTypeConfiguration<Square>
    {
        public void Configure(EntityTypeBuilder<Square> builder)
        {
            builder.ToTable("Sqaures");

            builder.Property(s => s.File);
            builder.Property(s => s.Row);
            builder.HasOne(s => s.Occupier).WithOne().HasForeignKey<Square>("OccupierId").IsRequired(false);
            builder.Property<Guid>("Id");
            builder.HasKey("Id");
            builder.Property<Guid?>("OccupierId").IsRequired(false);
        }
    }
}
/*
            builder.ToTable("Sqaures");
            builder.Property(s => s.File);
            builder.Property<Guid>("PositionId");
            builder.Property(s => s.Row);
            builder.HasOne(s => s.Occupier).WithOne().IsRequired(false).HasForeignKey<Square>("OccupierId").IsRequired(false);
            builder.Property<Guid>("Id");
            builder.HasKey("Id");
            builder.Property<Guid?>("OccupierId").IsRequired(false);
 * */