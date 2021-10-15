using System.Diagnostics.CodeAnalysis;
using CodeCompanion.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nova.Identity.Entities;

namespace Nova.Identity.Data;

sealed class AppDbContext : CodeCompanionDbContext
{
    public AppDbContext([NotNull] DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Boundary> Boundaries => Set<Boundary>();
    public DbSet<ClientApp> ClientApps => Set<ClientApp>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<UserBoundary> UserBoundaries => Set<UserBoundary>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}