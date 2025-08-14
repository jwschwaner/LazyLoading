using LazyLoading.Domain.Matches;

namespace LazyLoading.Application.Abstractions;

public interface IMatchReadRepository
{
    Task<(IReadOnlyList<Match> Items, string? NextCursor)> GetMatchesPageAsync(int limit, string? cursor,
        CancellationToken ct);
}