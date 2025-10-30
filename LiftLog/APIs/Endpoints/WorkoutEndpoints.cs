using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LiftLog.Data;
using LiftLog.Models;

namespace LiftLog.APIs;

public static class WorkoutEndpoints
{
    public static IEndpointRouteBuilder MapWorkoutEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/workouts").WithTags("Workouts");

        group.MapGet("", async (
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to,
            LiftLogDbContext db) =>
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

            var q = db.Workouts.AsQueryable();
            if (from is not null) q = q.Where(w => w.Date >= from);
            if (to is not null) q = q.Where(w => w.Date <= to);

            var total = await q.CountAsync();

            var items = await q.OrderByDescending(w => w.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(w => new WorkoutDto(w.Id, w.Date, w.Notes, w.Sets.Count))
                .ToListAsync();

            return Results.Ok(new { page, pageSize, total, items });
        })
        .WithOpenApi(op => { op.Summary = "List workouts (paginated)"; return op; });

        group.MapGet("{id:int}", async (int id, LiftLogDbContext db) =>
        {
            var w = await db.Workouts
                .Include(x => x.Sets)
                .ThenInclude(s => s.Exercise)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (w is null) return Results.NotFound();

            var dto = new WorkoutDetailDto(
                w.Id, w.Date, w.Notes,
                w.Sets.OrderBy(s => s.Id)
                      .Select(s => new SetLogDto(s.Id, s.ExerciseId, s.Exercise!.Name, s.Reps, s.Weight))
                      .ToList()
            );
            return Results.Ok(dto);
        })
        .WithOpenApi(op => { op.Summary = "Get workout by id"; return op; });

        group.MapPost("", async (CreateWorkoutDto dto, LiftLogDbContext db) =>
        {
            var w = new Workout { Date = dto.Date, Notes = dto.Notes };
            db.Add(w);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/workouts/{w.Id}",
                new WorkoutDto(w.Id, w.Date, w.Notes, 0));
        })
        .WithOpenApi(op => { op.Summary = "Create workout"; return op; });

        group.MapPut("{id:int}", async (int id, UpdateWorkoutDto dto, LiftLogDbContext db) =>
        {
            var w = await db.Workouts.FindAsync(id);
            if (w is null) return Results.NotFound();

            w.Date = dto.Date;
            w.Notes = dto.Notes;
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithOpenApi(op => { op.Summary = "Update workout"; return op; });

        group.MapDelete("{id:int}", async (int id, LiftLogDbContext db) =>
        {
            var w = await db.Workouts.FindAsync(id);
            if (w is null) return Results.NotFound();

            db.Remove(w);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithOpenApi(op => { op.Summary = "Delete workout"; return op; });

        return routes;
    }
}
