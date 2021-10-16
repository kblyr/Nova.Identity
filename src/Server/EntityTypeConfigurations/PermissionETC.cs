using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.EntityTypeConfigurations
{
    sealed class PermissionETC : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(DatabaseInfo.Tables.Permission)
                .HasInsertFootprint()
                .HasUpdateFootprint()
                .HasDeleteFootprint();

            builder.HasOne(permission => permission.Boundary)
                .WithMany()
                .HasForeignKey(permission => permission.BoundaryId);
        }
    }
}