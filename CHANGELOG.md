# Changelog

All notable changes to this project are recorded here.
The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project follows [Semantic Versioning](https://semver.org/).

## [1.0.0]

First NuGet release. The package ID is `IcodeNet.FluentSpecification`; the namespace stays `FluentSpecification`.

### Added
- Targets .NET Standard 2.0, so it runs on .NET Framework 4.6.1+, .NET Core and .NET 5+ on Windows, Linux and macOS.
- XML documentation on every public type and member.
- Unit tests (xUnit) and a CI workflow.
- NuGet package metadata, SourceLink and symbol packages.

### Changed
- `And`, `Or` and `Not` now return `CompositeSpecification<T>`, so calls can be chained without a cast. Code that assigned the result to `SpecificationBase<T>` still compiles.
- `OrSpecification<T>` and `NotSpecification<T>` now derive from `CompositeSpecification<T>`, like `AndSpecification<T>` already did.
- Moved from a .NET Framework 4.5.2 project to an SDK-style project.
