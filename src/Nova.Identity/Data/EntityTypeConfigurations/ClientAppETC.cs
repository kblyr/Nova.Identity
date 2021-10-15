using CodeCompanion.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.Data.EntityTypeConfigurations;

sealed class ClientAppETC : IEntityTypeConfiguration<ClientApp>
{
    public void Configure(EntityTypeBuilder<ClientApp> builder)
    {
        builder.ToTable(Tables.ClientApp)
            .AsSoftDeletable()
            .HasInsertFootprint()
            .HasUpdateFootprint()
            .HasDeleteFootprint();

        builder.HasOne(_ => _.Boundary)
            .WithMany()
            .HasForeignKey(_ => _.BoundaryId);
    }
}