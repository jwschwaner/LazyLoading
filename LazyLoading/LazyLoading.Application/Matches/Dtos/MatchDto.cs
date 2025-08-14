using System.Runtime.InteropServices.JavaScript;

namespace LazyLoading.Application.Matches.Dtos;

public sealed record MatchDto(
    Guid Id,
    DateTimeOffset KickoffAt,
    string HomeTeam,
    string AwayTeam,
    int HomeScore,
    int AwayScore);