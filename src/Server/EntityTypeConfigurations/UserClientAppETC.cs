using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.EntityTypeConfigurations
{
    sealed class UserClientAppETC : IEntityTypeConfiguration<UserClientApp>
    {
        public void Configure(EntityTypeBuilder<UserClientApp> builder)
        {
            builder.ToTable(DatabaseInfo.Tables.UserClientApp)
                .HasInsertFootprint()
                .HasDeleteFootprint();

            builder.HasOne(userClientApp => userClientApp.User)
                .WithMany()
                .HasForeignKey(userClientApp => userClientApp.UserId);

            builder.HasOne(userClientApp => userClientApp.ClientApp)
                .WithMany()
                .HasForeignKey(userClientApp => userClientApp.ClientAppId);
        }
    }
}