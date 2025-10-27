import Foundation

extension APIClient {
    // GET /api/v1/workouts/{id}
    func fetchWorkout(id: Int) async throws -> Workout {
        try await get("api/v1/workouts/\(id)")
    }
}
