using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LiftLog.Data;
using LiftLog.Models;

namespace LiftLog.APIs;

public static class ExercisesEndpoints
{
    public static IEndpointRouteBuilder MapExercisesEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/exercises").WithTags("Exercises");

        group.MapGet("", async ([FromQuery] string? search, LiftLogDbContext db) =>
        {
            var q = db.Exercises.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(e => EF.Functions.Like(e.Name, $"%{search}%"));

            var list = await q.OrderBy(e => e.Name)
                              .Select(e => new ExerciseDto(e.Id, e.Name, e.MuscleGroup))
                              .ToListAsync();
            return Results.Ok(list);
        })
        .WithOpenApi(op => { op.Summary = "List/search exercises"; return op; });

        group.MapPost("", async ([FromBody] CreateExerciseDto dto, LiftLogDbContext db) =>
        {
            var exists = await db.Exercises.AnyAsync(e => e.Name == dto.Name);
            if (exists) return Results.Conflict(new ProblemDetails { Title = "Exercise already exists" });

            var e = new Exercise { Name = dto.Name, MuscleGroup = dto.MuscleGroup };
            db.Add(e);
            await db.SaveChangesAsync();
            return Results.Created($"/api/v1/exercises/{e.Id}",
                new ExerciseDto(e.Id, e.Name, e.MuscleGroup));
        })
        .WithOpenApi(op => { op.Summary = "Create exercise"; return op; });

        group.MapPut("{id:int}", async (int id, UpdateExerciseDto dto, LiftLogDbContext db) =>
        {
            var e = await db.Exercises.FindAsync(id);
            if (e is null) return Results.NotFound();

            e.Name = dto.Name;
            e.MuscleGroup = dto.MuscleGroup;
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithOpenApi(op => { op.Summary = "Update exercise"; return op; });

        group.MapDelete("{id:int}", async (int id, LiftLogDbContext db) =>
        {
            var e = await db.Exercises.FindAsync(id);
            if (e is null) return Results.NotFound();
            db.Remove(e);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithOpenApi(op => { op.Summary = "Delete exercise"; return op; });

        return routes;
    }
}
