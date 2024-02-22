using Chijimu.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Chijimu.Data.Contexts;

public class UrlContext : DbContext
{
    public DbSet<Url> Urls { get; set; }

    public UrlContext(DbContextOptions<UrlContext> contextOptions)
        : base(contextOptions)
    {
        Database.EnsureCreated();
    }

    public static DbContextOptionsBuilder SetConnectionString(DbContextOptionsBuilder optionsBuilder, IConfiguration config)
    {
        return optionsBuilder.UseSqlServer(config.GetConnectionString("ChijimuDb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Url>(e => e.HasIndex(e => e.ShortUrlIdentifier).IsUnique());
    }
}