using LazyLoading.Application.Abstractions;
using LazyLoading.Application.Common.Pagination;
using LazyLoading.Application.Matches.Dtos;
using MediatR;

namespace LazyLoading.Application.Matches.Queries.GetMatchesPage;

public class GetMatchesPageQueryHandler(IMatchReadRepository repo)
    : IRequestHandler<GetMatchesPageQuery, PagedResponse<MatchDto>>
{
    public async Task<PagedResponse<MatchDto>> Handle(GetMatchesPageQuery request, CancellationToken ct)
    {
        var (items, next) = await repo.GetMatchesPageAsync(request.Limit, request.Cursor, ct);
        
        var dtos = items.Select(m => new MatchDto(m.Id, m.KickoffAt, m.HomeTeam, m.AwayTeam, m.HomeScore, m.AwayScore))
            .ToList();

        return new PagedResponse<MatchDto>(dtos, next);
    }
}