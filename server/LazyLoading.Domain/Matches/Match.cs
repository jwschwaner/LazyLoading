namespace LazyLoading.Domain.Matches;

public sealed class Match
{
    public Guid Id { get; private set; }
    public DateTimeOffset KickoffAt { get; private set; }
    public string HomeTeam { get; private set; } = null!;
    public string AwayTeam { get; private set; } = null!;
    public int HomeScore { get; private set; }
    public int AwayScore { get; private set; }

    private Match()
    {
    }

    public Match(Guid id, DateTimeOffset kickoffAt, string homeTeam, string awayTeam, int homeScore, int awayScore)
    {
        if (homeScore < 0 || awayScore < 0)
            throw new ArgumentOutOfRangeException("Scores must be non-negative.");
        
        Id = id;
        KickoffAt = kickoffAt;
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
        HomeScore = homeScore;
        AwayScore = awayScore;
    }
}