# Contributing

Thank you for helping improve FluentSpecification.

## Build and test

You need the [.NET SDK](https://dotnet.microsoft.com/download) 10.0 or later. It works on Windows, Linux and macOS.

```sh
dotnet build -c Release
dotnet test -c Release
dotnet run --project FluentSpecification.Samples
```

## Pull requests

1. Open an issue first for anything larger than a small fix.
2. Add or update tests for every behaviour change.
3. Document every new public type or member with XML comments. The build fails on missing docs in CI.
4. Update `CHANGELOG.md` under an `Unreleased` heading.
5. Keep one logical change per pull request.

## Releasing (maintainers)

1. Set `<Version>` in `FluentSpecification/FluentSpecification.csproj` and move the changelog entries under that version.
2. Merge to `master`, then push a tag such as `v1.0.1`.
3. The `release` workflow builds, tests and pushes the package to NuGet using [Trusted Publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing). GitHub proves the workflow's identity to nuget.org, which returns a one-hour API key. No long-lived key is stored.

One-time setup on nuget.org (Trusted Publishing page, under your user menu):

- Repository owner: `IcodeNet`
- Repository: `FluentSpecification`
- Workflow file: `release.yml`
- Package glob: `IcodeNet.FluentSpecification*`, with the scope that allows pushing new packages and new versions

Then add your nuget.org profile name (not your email) as the repository secret `NUGET_USER`.
