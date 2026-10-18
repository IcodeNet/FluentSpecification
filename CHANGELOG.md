# Changelog

All notable changes to this project are recorded here.
The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project follows [Semantic Versioning](https://semver.org/).

## [Unreleased]

## [1.1.1]

### Fixed
- The README logo is a Markdown image, so it renders on nuget.org instead of showing raw HTML.

## [1.1.0]

### Added
- `ExpressionSpecification<T>`: a rule built from an expression tree. Entity Framework translates it to SQL. Tested against SQLite.
- Nullable reference type annotations in the public API.
- Package validation against 1.0.0, so a release cannot break the public API by accident.
- The release workflow creates a GitHub Release with generated notes.

### Changed
- `IsSatisfiedBy` is now `virtual`. The built-in specifications check one item directly instead of running a query, which is far faster in loops. Results are unchanged.
- Constructors, `And`, `Or` (naming `otherCandidate`) and `SatisfyingElementsFrom` now throw `ArgumentNullException` for `null` arguments. Before, a `null` failed later with a `NullReferenceException` or silently built a broken rule.
- `FunctionalSpecification<T>` no longer adds a redundant `Select`.

### Fixed
- The 1.0.0 documentation also said combinators compare items by default equality on every provider; on a database they compare whole rows. The documentation now says so.
- The 1.0.0 documentation said database providers filter a `FunctionalSpecification<T>` in memory. They do not: they throw `InvalidOperationException`. The documentation now says so and points to `ExpressionSpecification<T>`.

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
