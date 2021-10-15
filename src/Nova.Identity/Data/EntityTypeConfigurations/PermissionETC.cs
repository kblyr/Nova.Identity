using CodeCompanion.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.Data.EntityTypeConfigurations;

sealed class PermissionETC : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(Tables.Permission)
            .AsSoftDeletable()
            .HasInsertFootprint()
            .HasUpdateFootprint()
            .HasDeleteFootprint();

        builder.HasOne(_ => _.Boundary)
            .WithMany()
            .HasForeignKey(_ => _.BoundaryId);
    }
}