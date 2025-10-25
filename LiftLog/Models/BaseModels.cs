namespace LiftLog.Models;

public class Workout
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string? Notes { get; set; }

    public List<SetLog> Sets { get; set; } = new();
}

public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string? MuscleGroup { get; set; }
}

public class SetLog
{
    public int Id { get; set; }
    public int WorkoutId { get; set; }
    public int ExerciseId { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; } // weight in lb

    public Workout? Workout { get; set; }
    public Exercise? Exercise { get; set; }
}
