import SwiftUI

struct WorkoutDetailView: View {
    @StateObject private var vm: WorkoutViewModel

    init(api: APIClient, workoutId: Int) {
        _vm = StateObject(wrappedValue: WorkoutViewModel(api: api, workoutId: workoutId))
    }

    var body: some View {
        Group {
            if vm.isLoading && vm.workout == nil {
                ProgressView("Loading…")
            } else if let w = vm.workout {
                List {
                    Section {
                        LabeledContent("Date", value: w.date.formatted(date: .abbreviated, time: .omitted))
                        if let notes = w.notes, !notes.isEmpty {
                            VStack(alignment: .leading, spacing: 6) {
                                Text("Notes").font(.subheadline).foregroundStyle(.secondary)
                                Text(notes)
                            }
                        }
                    }

                    Section("Sets") {
                        ForEach(w.sets) { s in
                            HStack {
                                VStack(alignment: .leading) {
                                    Text(s.exerciseName).font(.headline)
                                    Text("\(s.reps) reps").font(.caption).foregroundStyle(.secondary)
                                }
                                Spacer()
                                Text("\(s.weight, specifier: "%.1f")")
                                    .accessibilityLabel("Weight \(s.weight)")
                            }
                        }
                    }
                }
                .listStyle(.insetGrouped)
            } else {
                ContentUnavailableView("Couldn’t load workout", systemImage: "exclamationmark.triangle", description: Text(vm.error ?? "Unknown error"))
            }
        }
        .navigationTitle("Workout")
        .task { await vm.load() }
        .alert("Error", isPresented: .constant(vm.error != nil)) {
            Button("OK") { vm.error = nil }
        } message: { Text(vm.error ?? "") }
    }
}
