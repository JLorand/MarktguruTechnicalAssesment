using System.Reflection;
using Marktguru.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Marktguru.Infrastructure.Data;

public class MarktguruDbContext(DbContextOptions<MarktguruDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}