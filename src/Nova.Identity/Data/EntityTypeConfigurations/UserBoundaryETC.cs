using CodeCompanion.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.Data.EntityTypeConfigurations;

sealed class UserBoundaryETC : IEntityTypeConfiguration<UserBoundary>
{
    public void Configure(EntityTypeBuilder<UserBoundary> builder)
    {
        builder.ToTable(Tables.UserBoundary)
            .AsSoftDeletable()
            .HasInsertFootprint()
            .HasDeleteFootprint();

        builder.HasOne(_ => _.User)
            .WithMany()
            .HasForeignKey(_ => _.UserId);

        builder.HasOne(_ => _.Boundary)
            .WithMany()
            .HasForeignKey(_ => _.BoundaryId);
    }
}