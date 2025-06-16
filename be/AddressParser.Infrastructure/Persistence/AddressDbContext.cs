using AddressParser.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AddressParser.Infrastructure.Persistence;

public class AddressDbContext : DbContext
{
    public DbSet<ParsedAddress> Addresses => Set<ParsedAddress>();
    public DbSet<ParseStatistics> ParseStatistics { get; set; }

    public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParsedAddress>().HasKey(p => p.Id);
        base.OnModelCreating(modelBuilder);
    }
}
