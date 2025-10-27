//
//  WorkoutViewModel.swift
//  LiftLog
//
//  Created by Alexander Solivais on 10/27/25.
//


import Foundation

@MainActor
final class WorkoutViewModel: ObservableObject {
    @Published private(set) var workout: Workout?
    @Published var isLoading = false
    @Published var error: String?

    private let api: APIClient
    private let workoutId: Int

    init(api: APIClient, workoutId: Int) {
        self.api = api
        self.workoutId = workoutId
    }

    func load() async {
        guard !isLoading else { return }
        isLoading = true; defer { isLoading = false }
        do {
            let w = try await api.fetchWorkout(id: workoutId)
            workout = w
            error = nil
        } catch {
            self.error = (error as? LocalizedError)?.errorDescription ?? error.localizedDescription
        }
    }
}
