import SwiftUI

@main
struct LiftLogApp: App {
    var body: some Scene {
        WindowGroup {
            let base = URL(string: "https://localhost:5001")! // change to your base
            let client = APIClient(config: .init(baseURL: base, getToken: { nil }))
            NavigationStack {
                WorkoutDetailView(api: client, workoutId: 1)
            }
        }
    }
}
