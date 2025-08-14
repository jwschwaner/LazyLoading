using LazyLoading.Application.Abstractions;
using LazyLoading.Application.Matches.Queries.GetMatchesPage;
using LazyLoading.Infrastructure.Persistence;
using LazyLoading.Infrastructure.Repositories;
using LazyLoading.Infrastructure.Seeding;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("LazyLoading.Infrastructure"))
);
    
builder.Services.AddMediatR(typeof(GetMatchesPageQuery));
builder.Services.AddScoped<IMatchReadRepository, MatchReadRepository>();

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p =>
        p.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.MapGet("/api/matches", async (int? limit, string? cursor, IMediator mediator, CancellationToken ct) =>
{
    var page = Math.Clamp(limit ?? 100, 1, 100);
    var result = await mediator.Send(new GetMatchesPageQuery(page, cursor), ct);
    return Results.Ok(result);
});

app.MapPost("api/matches/seed", async (int? count, AppDbContext dbContext, CancellationToken ct) =>
{
    var n = Math.Clamp(count ?? 100, 1, 10000);
    var added = await MatchSeeder.SeedAsync(dbContext, n, null, ct);
    return Results.Ok(new { added });
});

app.MapDelete("api/matches", async (AppDbContext dbContext, CancellationToken ct) =>
{
    var deleted = await dbContext.Matches.ExecuteDeleteAsync(ct);
    return Results.Ok(new { deleted });
});

app.Run();