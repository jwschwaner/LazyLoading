using LazyLoading.Application.Common.Pagination;
using LazyLoading.Application.Matches.Dtos;
using MediatR;

namespace LazyLoading.Application.Matches.Queries.GetMatchesPage;

public record GetMatchesPageQuery(int Limit = 10, string? Cursor = null) : IRequest<PagedResponse<MatchDto>>;