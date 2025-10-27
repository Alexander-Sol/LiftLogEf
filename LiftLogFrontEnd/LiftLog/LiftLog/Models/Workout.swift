//
//  WorkoutSet.swift
//  LiftLog
//
//  Created by Alexander Solivais on 10/27/25.
//


import Foundation

struct WorkoutSet: Identifiable, Codable, Equatable {
    let id: Int
    let exerciseId: Int
    let exerciseName: String
    let reps: Int
    let weight: Double
}

struct Workout: Identifiable, Codable, Equatable {
    let id: Int
    let date: Date
    var notes: String?
    var sets: [WorkoutSet]

    enum CodingKeys: String, CodingKey { case id, date, notes, sets }

    // Your API returns date as "YYYY-MM-DD" (no time zone); decode it explicitly.
    init(from decoder: Decoder) throws {
        let c = try decoder.container(keyedBy: CodingKeys.self)
        id = try c.decode(Int.self, forKey: .id)
        notes = try c.decodeIfPresent(String.self, forKey: .notes)
        sets = try c.decode([WorkoutSet].self, forKey: .sets)

        let raw = try c.decode(String.self, forKey: .date)
        guard let d = Self.yyyyMMdd.date(from: raw) else {
            throw DecodingError.dataCorruptedError(forKey: .date, in: c,
                debugDescription: "Expected date in yyyy-MM-dd, got \(raw)")
        }
        date = d
    }

    static let yyyyMMdd: DateFormatter = {
        let f = DateFormatter()
        f.calendar = Calendar(identifier: .gregorian)
        f.locale = Locale(identifier: "en_US_POSIX")
        f.dateFormat = "yyyy-MM-dd"
        return f
    }()
}
