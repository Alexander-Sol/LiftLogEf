//
//  LiftsViewModel.swift
//  LiftLog
//
//  Created by Alexander Solivais on 10/27/25.
//


import Foundation

@MainActor
final class LiftsViewModel: ObservableObject {
    @Published private(set) var entries: [LiftLogEntry] = []
    @Published var isLoading = false
    @Published var error: String?
    @Published var hasMore = true

    private let api: APIClient
    private var page = 1
    private let pageSize = 20

    init(api: APIClient) {
        self.api = api
    }

    func loadInitial() async {
        guard !isLoading else { return }
        isLoading = true; defer { isLoading = false }
        page = 1
        do {
            let res: PagedResponse<LiftLogEntry> = try await api.get(
                "api/lifts",
                query: [
                    .init(name: "page", value: "\(page)"),
                    .init(name: "pageSize", value: "\(pageSize)")
                ]
            )
            entries = res.data
            hasMore = entries.count < res.totalCount
            error = nil
        } catch {
            self.error = (error as? APIError)?.localizedDescription ?? error.localizedDescription
        }
    }

    func loadMoreIfNeeded(current item: LiftLogEntry?) async {
        guard hasMore, !isLoading else { return }
        guard let item, let idx = entries.firstIndex(where: { $0.id == item.id }),
              idx >= entries.count - 5 else { return }

        isLoading = true; defer { isLoading = false }
        page += 1
        do {
            let res: PagedResponse<LiftLogEntry> = try await api.get(
                "api/lifts",
                query: [
                    .init(name: "page", value: "\(page)"),
                    .init(name: "pageSize", value: "\(pageSize)")
                ]
            )
            entries.append(contentsOf: res.data)
            hasMore = entries.count < res.totalCount
        } catch {
            self.error = (error as? APIError)?.localizedDescription ?? error.localizedDescription
        }
    }

    func add(entry: LiftLogEntry) async {
        do {
            let created: LiftLogEntry = try await api.post("api/lifts", body: entry)
            entries.insert(created, at: 0)
        } catch {
            self.error = (error as? APIError)?.localizedDescription ?? error.localizedDescription
        }
    }
}
