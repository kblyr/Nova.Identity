using CodeCompanion.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.Data.EntityTypeConfigurations;

sealed class BoundaryETC : IEntityTypeConfiguration<Boundary>
{
    public void Configure(EntityTypeBuilder<Boundary> builder)
    {
        builder.ToTable(Tables.Boundary)
            .AsSoftDeletable()
            .HasInsertFootprint()
            .HasUpdateFootprint()
            .HasDeleteFootprint();
    }
}