using System.Reflection;
using Marktguru.Application.Interfaces;
using Marktguru.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Marktguru.Infrastructure.Data;

public class MarktguruDbContext(DbContextOptions<MarktguruDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Product> Products => Set<Product>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}