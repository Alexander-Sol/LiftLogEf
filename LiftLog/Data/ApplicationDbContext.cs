using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LiftLog.Models;

namespace LiftLog.Data;

public class LiftLogDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<SetLog> Sets => Set<SetLog>();
    public LiftLogDbContext(DbContextOptions<LiftLogDbContext> options)
        : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder b)
        => b.UseSqlite("Data Source=liftlog.db");

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Workout>()
            .HasMany(w => w.Sets)
            .WithOne(s => s.Workout!)
            .HasForeignKey(s => s.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Exercise>()
            .HasIndex(e => e.Name).IsUnique();
    }
}

public record User(int Id, string Email, string? DisplayName);
public record Exercise(int Id, string Name, string? MuscleGroup);
public record WorkoutExercise(int Id, int WorkoutId, int ExerciseId, int OrderIndex, int? TargetSets, int? TargetReps);
public record SetLog(int Id, int WorkoutExerciseId, int SetIndex, int? Reps, double? Weight, double? Rpe, DateTime CreatedAt);

