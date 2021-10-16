using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.EntityTypeConfigurations
{
    sealed class BoundaryETC : IEntityTypeConfiguration<Boundary>
    {
        public void Configure(EntityTypeBuilder<Boundary> builder)
        {
            builder.ToTable(DatabaseInfo.Tables.Boundary)
                .HasInsertFootprint()
                .HasUpdateFootprint()
                .HasDeleteFootprint();
        }
    }
}