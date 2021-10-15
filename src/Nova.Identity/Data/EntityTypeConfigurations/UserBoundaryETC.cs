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
    }
}