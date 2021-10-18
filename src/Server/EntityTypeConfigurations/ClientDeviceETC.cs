using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.EntityTypeConfigurations
{
    sealed class ClientDeviceETC : IEntityTypeConfiguration<ClientDevice>
    {
        public void Configure(EntityTypeBuilder<ClientDevice> builder)
        {
            builder.ToTable(DatabaseInfo.Tables.ClientDevice)
                .HasInsertFootprint()
                .HasUpdateFootprint();
        }
    }
}