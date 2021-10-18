using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.EntityTypeConfigurations
{
    sealed class UserClientDeviceETC : IEntityTypeConfiguration<UserClientDevice>
    {
        public void Configure(EntityTypeBuilder<UserClientDevice> builder)
        {
            builder.ToTable(DatabaseInfo.Tables.UserClientDevice)
                .HasInsertFootprint()
                .HasDeleteFootprint();

            builder.HasOne(userClientDevice => userClientDevice.User)
                .WithMany()
                .HasForeignKey(userClientDevice => userClientDevice.UserId);

            builder.HasOne(userClientDevice => userClientDevice.ClientDevice)
                .WithMany()
                .HasForeignKey(userClientDevice => userClientDevice.ClientDeviceId);
        }
    }
}