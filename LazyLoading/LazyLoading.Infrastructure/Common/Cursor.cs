using System.Text;
using System.Text.Json;

namespace LazyLoading.Infrastructure.Common;

public static class Cursor
{
    private record Payload(long Ticks, Guid Id);

    public static string Encode(DateTimeOffset kickoffAt, Guid id)
    {
        var json = JsonSerializer.Serialize(new Payload(kickoffAt.UtcTicks, id));
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }

    public static (DateTimeOffset kickoffAt, Guid id)? Decode(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor)) return null;

        try
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
            var p = JsonSerializer.Deserialize<Payload>(json)!;
            return (new DateTimeOffset(p.Ticks, TimeSpan.Zero), p.Id);
        }
        catch
        {
            return null;
        }
    }
}