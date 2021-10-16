using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.EntityTypeConfigurations
{
    sealed class ClientAppETC : IEntityTypeConfiguration<ClientApp>
    {
        public void Configure(EntityTypeBuilder<ClientApp> builder)
        {
            builder.ToTable(DatabaseInfo.Tables.ClientApp)
                .HasInsertFootprint()
                .HasUpdateFootprint()
                .HasDeleteFootprint();

            builder.HasOne(clientApp => clientApp.Boundary)
                .WithMany()
                .HasForeignKey(clientApp => clientApp.BoundaryId);
        }
    }
}