using LazyLoading.Application.Abstractions;
using LazyLoading.Domain.Matches;
using LazyLoading.Infrastructure.Common;
using LazyLoading.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LazyLoading.Infrastructure.Repositories;

public sealed class MatchReadRepository(AppDbContext dbContext) : IMatchReadRepository
{
    public async Task<(IReadOnlyList<Match> Items, string? NextCursor)> GetMatchesPageAsync(int limit, string? cursor,
        CancellationToken ct)
    {
        var decoded = Cursor.Decode(cursor);
        var baseQuery = dbContext.Matches.AsNoTracking();

        if (decoded is not null)
        {
            var (cutoff, id) = decoded.Value;
            var idText = id.ToString();
            
            baseQuery = baseQuery.Where(m =>
                m.KickoffAt < cutoff || 
                (m.KickoffAt == cutoff && string.Compare(m.Id.ToString(), idText) < 0));
        }

        var query = baseQuery
            .OrderByDescending(m => m.KickoffAt)
            .ThenByDescending(m => m.Id);

        var items = await query.Take(limit).ToListAsync(ct);
        string? next = null;
        if (items.Count != limit) return (items, next);
        var last = items[^1];
        next = Cursor.Encode(last.KickoffAt, last.Id);

        return (items, next);
    }
}