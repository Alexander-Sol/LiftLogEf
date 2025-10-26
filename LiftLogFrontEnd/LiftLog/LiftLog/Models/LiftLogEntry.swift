import Foundation

struct LiftLogEntry: Identifiable, Codable, Equatable {
    let id: UUID
    var date: Date
    var exercise: String
    var sets: Int
    var reps: Int
    var weight: Double
}

struct PagedResponse<T: Codable>: Codable {
    let data: [T]
    let totalCount: Int
    let page: Int
    let pageSize: Int
}
