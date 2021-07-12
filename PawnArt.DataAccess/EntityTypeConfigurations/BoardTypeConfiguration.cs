using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PawnArt.Logic.BoardArrangment;

namespace PawnArt.DataAccess.EntityTypeConfigurations
{
    public class BoardTypeConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.ToTable("Boards");

            builder.Property<Guid>("CurrentPositionId");


            builder.Property<Guid>("Id");
            builder.HasKey("Id");
            builder.Property<Guid>("GameId");
        }
    }
}
