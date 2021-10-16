using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.EntityTypeConfigurations
{
    sealed class RoleETC : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(DatabaseInfo.Tables.Role)
                .HasInsertFootprint()
                .HasUpdateFootprint()
                .HasDeleteFootprint();

            builder.HasOne(role => role.Boundary)
                .WithMany()
                .HasForeignKey(role => role.BoundaryId);
        }
    }
}