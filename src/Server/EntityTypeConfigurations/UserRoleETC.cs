using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.EntityTypeConfigurations
{
    sealed class UserRoleETC : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable(DatabaseInfo.Tables.UserRole)
                .HasInsertFootprint()
                .HasDeleteFootprint();

            builder.HasOne(userRole => userRole.User)
                .WithMany()
                .HasForeignKey(userRole => userRole.UserId);

            builder.HasOne(userRole => userRole.Role)
                .WithMany()
                .HasForeignKey(userRole => userRole.RoleId);
        }
    }
}