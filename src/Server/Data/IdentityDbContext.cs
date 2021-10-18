using System.Diagnostics.CodeAnalysis;
using CodeCompanion.EntityFrameworkCore;
using CodeCompanion.Processes;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Entities;

namespace Nova.Identity.Data
{
    sealed class IdentityDbContext : CodeCompanionDbContext, IProcessContext
    {
        public IdentityDbContext([NotNull] DbContextOptions options) : base(options) { }

        public DbSet<Boundary> Boundaries => Set<Boundary>();
        public DbSet<ClientApp> ClientApps => Set<ClientApp>();
        public DbSet<ClientDevice> ClientDevices => Set<ClientDevice>();
        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserClientApp> UserClientApps => Set<UserClientApp>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(DatabaseInfo.Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
        }
    }
}