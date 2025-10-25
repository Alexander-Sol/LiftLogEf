using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LiftLog.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<SetLog> Sets => Set<SetLog>();
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder b)
        => b.UseSqlite("Data Source=liftlog.db");
}

public record User(int Id, string Email, string? DisplayName);
public record Workout(int Id, int UserId, string Name, DateOnly? PlannedDate);
public record Exercise(int Id, string Name, string? MuscleGroup);
public record WorkoutExercise(int Id, int WorkoutId, int ExerciseId, int OrderIndex, int? TargetSets, int? TargetReps);
public record SetLog(int Id, int WorkoutExerciseId, int SetIndex, int? Reps, double? Weight, double? Rpe, DateTime CreatedAt);

