namespace LiftLog.Models;

public record WorkoutDto(int Id, DateOnly Date, string? Notes, int SetCount);
public record WorkoutDetailDto(int Id, DateOnly Date, string? Notes, List<SetDto> Sets);
public record CreateWorkoutDto(DateOnly Date, string? Notes);
public record UpdateWorkoutDto(DateOnly Date, string? Notes);

public record ExerciseDto(int Id, string Name, string? MuscleGroup);
public record CreateExerciseDto(string Name, string? MuscleGroup);
public record UpdateExerciseDto(string Name, string? MuscleGroup);

public record SetDto(int Id, int ExerciseId, string ExerciseName, int Reps, decimal Weight);
public record CreateSetDto(int ExerciseId, int Reps, decimal Weight);
public record UpdateSetDto(int ExerciseId, int Reps, decimal Weight);
