using CodeCompanion.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.Data.EntityTypeConfigurations;

sealed class UserRoleETC : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(Tables.UserRole)
            .AsSoftDeletable()
            .HasInsertFootprint()
            .HasDeleteFootprint();
    }
}