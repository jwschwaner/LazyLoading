using LazyLoading.Domain.Matches;
using Microsoft.EntityFrameworkCore;

namespace LazyLoading.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>(b =>
        {
            b.HasKey(m => m.Id);
            b.Property(m => m.HomeTeam).HasMaxLength(120);
            b.Property(m => m.AwayTeam).HasMaxLength(120);
            b.HasIndex(m => m.KickoffAt);
        });
    }
}