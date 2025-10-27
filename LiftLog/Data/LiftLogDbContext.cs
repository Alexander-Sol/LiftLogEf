using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LiftLog.Models;

namespace LiftLog.Data;


public class LiftLogDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    //public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();
    public DbSet<SetLog> Sets => Set<SetLog>();
    public LiftLogDbContext(DbContextOptions<LiftLogDbContext> options)
        : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder b)
        => b.UseSqlite("Data Source=liftlog.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Workout>()
            .HasMany(w => w.Sets)
            .WithOne(s => s.Workout!)
            .HasForeignKey(s => s.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Exercise>()
            .HasIndex(e => e.Name).IsUnique();

        // Seed Exercises
        modelBuilder.Entity<Exercise>().HasData(
            new Exercise { Id = 1, Name = "Bench Press", MuscleGroup = "Chest" },
            new Exercise { Id = 2, Name = "Squat", MuscleGroup = "Legs" }
        );

        // Seed Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Email = "user@example.com", DisplayName = "John Doe" }
        );

        // Seed Workouts
        modelBuilder.Entity<Workout>().HasData(
            new Workout { Id = 1, UserId = 1, Date = new DateOnly(2025, 10, 26) }
        );

        // Seed WorkoutSets
        modelBuilder.Entity<SetLog>().HasData(
            new SetLog { Id = 1, WorkoutId = 1, ExerciseId = 1, Reps = 8, Weight = 135 },
            new SetLog { Id = 2, WorkoutId = 1, ExerciseId = 2, Reps = 5, Weight = 185 }
        );
    }
}

//public record User(int Id, string Email, string? DisplayName);
// public record Exercise(int Id, string Name, string? MuscleGroup);
// public record WorkoutExercise(int Id, int WorkoutId, int ExerciseId, int OrderIndex, int? TargetSets, int? TargetReps);
// public record SetLog(int Id, int WorkoutExerciseId, int SetIndex, int? Reps, double? Weight, double? Rpe, DateTime CreatedAt);

