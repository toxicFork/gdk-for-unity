# Changelog

## Unreleased

### Added

- Added support for the Steam authentication flow.

### Changed

- Upgraded the Worker SDK snapshot version.
- `Improbable.Gdk.EntityTemplate` is now mutable and exposes a set of APIs to add, remove, and replace component snapshots
    - This replaces the `Improbable.Gdk.Core.EntityBuilder` class.
    - These changes also allow you to reuse an `EntityTemplate` more than once.

### Fixed

- `Clean all workers` now cleans worker configs in addition to built-out workers.
- Fixed a bug where you could start each built-out worker only once on OSX.
- Code generation now captures nested package dependencies, so the generated schema contains schema components from all required packages. Previously, code generation only generated schema for top-level dependencies, skipping nested packages.

### Removed

- Removed the `Improbable.Gdk.Core.EntityBuilder` class as it was superceded by the updated functionality in `Improbable.Gdk.Core.EntityTemplate`.
    - Removed `CreateSchemaComponentData` from each generated component as it is no longer required by the `EntityBuilder`.

## `0.1.3` - 2018-11-26

### Added

- Added Frames Per Second (FPS) and Unity heap usage as metrics sent by `MetricSendSystem.cs`.
- Added a warning message to the top of schema files copied into the `from_gdk_packages` directory.
- Added an `ISnapshottable<T>` interface to all generated components. This allows you to convert a component to a snapshot.
- Added an `EntityId` property on the Readers/Writers to access the `EntityId` of the underlying SpatialOS entity.
- Added a `HasEntity` method to the `WorkerSystem`. This allows you to check if an entity is checked out on your worker.
- Added operators and conversion methods to `Coordinates`, `Vector3d`, and `Vector3f` in code generation.
    - This supercedes the `StandardLibraryUtils` feature module which was removed as a consequence.

### Changed

- Improved the method of calculating load and FPS.
- Updated test project Unity version to `2018.2.14f`.
- Upgraded the Worker SDK snapshot version. This entails the following changes:
    - `EntityId` is now in the `Improbable.Gdk.Core` namespace. (Previously `Improbable.Worker`).
    - `Dispatcher` is now in the `Improbable.Gdk.Core` namespace. (Previously `Improbable.Worker`).
    - The `Improbable.Worker.Core` namespace is now `Improbable.Worker.CInterop`.

### Fixed

- Fixed a bug where schema components with a field named `value` would generate invalid code.

### Removed

- Removed the `StandardLibraryUtils` feature module as it was superceded by inserting the methods during code generation.

## `0.1.2` - 2018-11-01

### Added

- Added the ability to acknowledge `AuthorityLossImminent` messages.
- Added an `Open Inspector` button to the `SpatialOS` menu in the Unity Editor.
- Added support for local mobile development.
- Added a changelog.
- Added field level dirty markers in components. This allows for partial automatic component updates to be sent.
- Added full support for `EntityQuery` world commands.
    - Added `Improbable.Gdk.Core.EntityQuerySnapshot` to hold the result of a single entity from a snapshot query.
    - Added `Improbable.Gdk.Core.ISpatialComponentSnapshot` to differentiate between a snapshot of component state and component data.

### Changed

- Changed the allocation type used internally for Unity ECS chunk iteration from `Temp` to `TempJob`
- Running a build in the Editor no longer automatically selects all scenes in the Unity build configuration
- `Improbable.Gdk.Core.Snapshot.AddEntity` now returns the `EntityId` assigned in the snapshot.
- Changed the `WorkerConnector` to be more generic and have an explicit `StandaloneWorkerConnector` for any workers running on OSX/Linux/Windows.
- Updated the default Unity version to `2018.2.14f1`.

### Fixed

- Fixed a bug where deserialising multiple events in a single component update only returned N copies of the last event received, where N is the number of events in the update.
- Fixed a broken link to the setup guide in an error message.

## `0.1.1` - 2018-10-19

### Added

- Better error messages when missing build support for a target platform.
- Better error messages for common problems when downloading the Worker SDK.

### Changed

- Position updates are now sent after all other updates.
- Simplified the heartbeating system in the `PlayerLifecycle` feature module.
- Updated the `README` and "Get Started" guide.

### Fixed

- The `GameLogic` worker is run in headless mode.
- The `Clean All Workers` menu item now works.

## `0.1.0` - 2018-10-10

The initial alpha release of the SpatialOS GDK for Unity.
