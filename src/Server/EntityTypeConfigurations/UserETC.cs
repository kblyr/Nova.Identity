using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nova.Identity.Entities;

namespace Nova.Identity.EntityTypeConfigurations
{
    sealed class UserETC : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(DatabaseInfo.Tables.User)
                .HasInsertFootprint()
                .HasUpdateFootprint()
                .HasDeleteFootprint();
        }
    }
}