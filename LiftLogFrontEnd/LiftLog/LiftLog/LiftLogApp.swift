//
//  LiftLogApp.swift
//  LiftLog
//
//  Created by Alexander Solivais on 10/26/25.
//

import SwiftUI
import CoreData

@main
struct LiftLogApp: App {
    let persistenceController = PersistenceController.shared

    var body: some Scene {
        WindowGroup {
            ContentView()
                .environment(\.managedObjectContext, persistenceController.container.viewContext)
        }
    }
}
