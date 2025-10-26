using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using LiftLog.Data;
using LiftLog.Models;

public static class SetsEndpoints
{
    public static IEndpointRouteBuilder MapSetsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/workouts").WithTags("Sets");

        group.MapPost("{workoutId:int}/sets", async (int workoutId, CreateSetDto dto, LiftLogDbContext db) =>
        {
            var workoutExists = await db.Workouts.AnyAsync(w => w.Id == workoutId);
            if (!workoutExists) return Results.NotFound(new ProblemDetails { Title = "Workout not found" });

            var exercise = await db.Exercises.FindAsync(dto.ExerciseId);
            if (exercise is null) return Results.BadRequest(new ProblemDetails { Title = "Invalid ExerciseId" });

            var set = new SetLog
            {
                WorkoutId = workoutId,
                ExerciseId = dto.ExerciseId,
                Reps = dto.Reps,
                Weight = dto.Weight
            };
            db.Add(set);
            await db.SaveChangesAsync();

            return Results.Created($"/api/v1/workouts/{workoutId}",
                new { set.Id, set.WorkoutId, set.ExerciseId, set.Reps, set.Weight });
        })
        .WithOpenApi(op => { op.Summary = "Add set to workout"; return op; });

        group.MapPut("{workoutId:int}/sets/{setId:int}", async (int workoutId, int setId, UpdateSetDto dto, LiftLogDbContext db) =>
        {
            var set = await db.Sets.FirstOrDefaultAsync(s => s.Id == setId && s.WorkoutId == workoutId);
            if (set is null) return Results.NotFound();

            var exerciseExists = await db.Exercises.AnyAsync(e => e.Id == dto.ExerciseId);
            if (!exerciseExists) return Results.BadRequest(new ProblemDetails { Title = "Invalid ExerciseId" });

            set.ExerciseId = dto.ExerciseId;
            set.Reps = dto.Reps;
            set.Weight = dto.Weight;

            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithOpenApi(op => { op.Summary = "Update set"; return op; });

        group.MapDelete("{workoutId:int}/sets/{setId:int}", async (int workoutId, int setId, LiftLogDbContext db) =>
        {
            var set = await db.Sets.FirstOrDefaultAsync(s => s.Id == setId && s.WorkoutId == workoutId);
            if (set is null) return Results.NotFound();

            db.Remove(set);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithOpenApi(op => { op.Summary = "Delete set"; return op; });

        return routes;
    }
}
