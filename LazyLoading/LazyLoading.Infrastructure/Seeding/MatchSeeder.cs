using LazyLoading.Domain.Matches;
using LazyLoading.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LazyLoading.Infrastructure.Seeding;

public static class MatchSeeder
{
    private static readonly string[] Teams =
    [
        "Lions", "Tigers", "Bears", "Wolves", "Hawks", "Eagles", "Sharks", "Panthers", "Dragons", "Falcons",
        "Ravens", "Foxes", "Rhinos", "Bulls", "Crows", "Owls", "Cheetahs", "Pythons", "Cobras", "Stallions"
    ];

    public static async Task<int> SeedAsync(AppDbContext dbContext, int count = 100, DateTimeOffset? now = null,
        CancellationToken ct = default)
    {
        now ??= DateTimeOffset.UtcNow;
        var rnd = new Random();

        for (var i = 0; i < count; i++)
        {
            var home = Teams[rnd.Next(Teams.Length)];
            string away;
            do
            {
                away = Teams[rnd.Next(Teams.Length)];
            } while (away == home);

            var kickOff = now.Value.AddDays(-rnd.Next(0, 100)).AddHours(rnd.Next(0, 24)).AddMinutes(rnd.Next(0, 60));
            var homeScore = rnd.Next(0, 6);
            var awayScore = rnd.Next(0, 6);
            
            var match = new Match(Guid.NewGuid(), kickOff, home, away, homeScore, awayScore);
            dbContext.Matches.Add(match);
        }
        
        await dbContext.SaveChangesAsync(ct);
        return count;
    }

    public static async Task<int> DeleteAllAsync(AppDbContext dbContext, CancellationToken ct = default)
    {
        return await dbContext.Matches.ExecuteDeleteAsync(ct);
    }
}