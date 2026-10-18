<p align="center">
  <img src="https://raw.githubusercontent.com/IcodeNet/FluentSpecification/master/docs/images/logo.png" alt="FluentSpecification" width="520">
</p>

[![ci](https://github.com/IcodeNet/FluentSpecification/actions/workflows/ci.yml/badge.svg)](https://github.com/IcodeNet/FluentSpecification/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/IcodeNet.FluentSpecification.svg)](https://www.nuget.org/packages/IcodeNet.FluentSpecification)
[![NuGet downloads](https://img.shields.io/nuget/dt/IcodeNet.FluentSpecification.svg)](https://www.nuget.org/packages/IcodeNet.FluentSpecification)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/IcodeNet/FluentSpecification/blob/master/LICENSE)
[![.NET Standard 2.0](https://img.shields.io/badge/.NET%20Standard-2.0-512bd4.svg)](https://learn.microsoft.com/dotnet/standard/net-standard)
[![Platforms](https://img.shields.io/badge/platforms-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey.svg)](#install)

A lightweight C# implementation of the **Specification pattern**. Express business rules as small, reusable, composable objects and combine them fluently with `And`, `Or` and `Not`.

## Install

```sh
dotnet add package IcodeNet.FluentSpecification
```

The package ID is `IcodeNet.FluentSpecification`. In code, the namespace stays `FluentSpecification`:

```csharp
using FluentSpecification;
```

Supported platforms: the package targets **.NET Standard 2.0**. It runs on .NET Framework 4.6.1+, .NET Core 2.0+ and .NET 5+, on Windows, Linux and macOS. It has no dependencies.

## Why?

Instead of scattering `if` conditions and LINQ predicates across your codebase, you express each rule as a named class (or a one-line lambda). Rules stay decoupled from your domain objects and can be combined without touching either.

## How it fits together

![Candidates flow through a specification and come out as matches](https://raw.githubusercontent.com/IcodeNet/FluentSpecification/master/docs/images/flow.png)

## Core Classes

| Class | Purpose |
|---|---|
| `SpecificationBase<T>` | Abstract base – implement `SatisfyingElementsFrom` to define your rule |
| `CompositeSpecification<T>` | Extends `SpecificationBase<T>` with `And`, `Or`, and `Not` combinators |
| `FunctionalSpecification<T>` | Ready-made specification built from a lambda – no subclass needed |

## Quick Start

### 1 – Define a named specification

Subclass `CompositeSpecification<T>` and express the rule in LINQ:

```csharp
public class OverDueLoanSpecification : CompositeSpecification<Loan>
{
    public override IQueryable<Loan> SatisfyingElementsFrom(IQueryable<Loan> candidates)
    {
        return from l in candidates
               where l.DateTaken < DateTime.Today
               where l.DateTaken.Add(l.LoanPeriod) < DateTime.Now
               select l;
    }
}
```

### 2 – Check a single object

```csharp
var customer = new Customer { FirstName = "Alice", Balance = 500.00 };
var valuedCustomer = new ValuedCustomerSpecification(); // Balance > 1300

bool isValued = valuedCustomer.IsSatisfiedBy(customer); // false
```

### 3 – Filter a collection

```csharp
var overdueSpec = new OverDueLoanSpecification();
IQueryable<Loan> overdueLoans = overdueSpec.SatisfyingElementsFrom(loans.AsQueryable());
```

### 4 – Use a lambda instead of a class

When the rule is simple enough that a dedicated class would be overkill, use `FunctionalSpecification<T>`:

```csharp
var bigLoanSpec = new FunctionalSpecification<Loan>(l => l.Amount > 10_000.00);
IQueryable<Loan> bigLoans = bigLoanSpec.SatisfyingElementsFrom(loans.AsQueryable());
```

### 5 – Combine specifications

![And, Or and Not shown as Venn diagrams](https://raw.githubusercontent.com/IcodeNet/FluentSpecification/master/docs/images/combinators.png)

Specifications that extend `CompositeSpecification<T>` (including `FunctionalSpecification<T>`) expose `And`, `Or`, and `Not`:

```csharp
var overdueSpec  = new OverDueLoanSpecification();
var bigLoanSpec  = new FunctionalSpecification<Loan>(l => l.Amount > 10_000.00);
var sixDaysSpec  = new FunctionalSpecification<Loan>(l => l.LoanPeriod == TimeSpan.FromDays(6));
var fortDaysSpec = new FunctionalSpecification<Loan>(l => l.LoanPeriod == TimeSpan.FromDays(14));

// AND – overdue AND large
var overdueAndBig = overdueSpec.And(bigLoanSpec);

// OR – 6-day or 14-day loans
var sixOrFourteen = sixDaysSpec.Or(fortDaysSpec);

// NOT – anything that is NOT a 6-day loan
var notSixDays = sixDaysSpec.Not();

// Use just like any other specification
IQueryable<Loan> results = repo.FindElementsBy(overdueAndBig);
```

Combinators can be chained arbitrarily:

```csharp
var complex = overdueSpec.And(bigLoanSpec).And(sixDaysSpec.Not());
```

## Repository Integration

Pass a specification into your repository to keep query logic out of your data layer:

```csharp
public class LoanRepository
{
    public IQueryable<Loan> FindElementsBy(SpecificationBase<Loan> spec)
    {
        // fetch your data source, then delegate filtering to the spec
        return spec.SatisfyingElementsFrom(loans.AsQueryable());
    }
}

// Caller decides the rule – repository stays unchanged
var results = repo.FindElementsBy(overdueSpec.And(bigLoanSpec));
```

## How it works, and what to watch for

Read this before you use the library with a database.

- **Each rule filters a set.** `SatisfyingElementsFrom` takes an `IQueryable<T>` and returns the items that pass. `IsSatisfiedBy` checks one item by running the same rule over a one-item set.
- **`And`, `Or` and `Not` use LINQ set operations** (`Intersect`, `Union` and `Except`). Items are compared with their default equality. A class without `Equals` and `GetHashCode` is compared by reference, so two different objects with the same data count as different items.
- **The combinators remove duplicates.** A single rule keeps duplicates. A combined rule does not.
- **`FunctionalSpecification<T>` takes a `Func<T, bool>`, not an expression.** Entity Framework and other query providers cannot translate it to SQL. They load the rows first and then filter in memory. For large tables, write a named specification with a LINQ expression inside `SatisfyingElementsFrom`, so the provider can translate the whole query.
- **Null arguments are not checked.** Passing `null` to a combinator or constructor fails later, when the rule runs.
- **Thread safety.** Specifications hold no changing state. They are safe to share across threads if your own predicates are.

## API reference

Full XML documentation ships with the package, so your editor shows it as you type.

| Member | Description |
|---|---|
| `SpecificationBase<T>.IsSatisfiedBy(T)` | Returns `true` when one item passes the rule |
| `SpecificationBase<T>.SatisfyingElementsFrom(IQueryable<T>)` | Abstract. Returns the items that pass the rule |
| `CompositeSpecification<T>.And(other)` | Both rules must pass |
| `CompositeSpecification<T>.Or(other)` | Either rule must pass |
| `CompositeSpecification<T>.Not()` | The rule must not pass |
| `FunctionalSpecification<T>(Func<T, bool>)` | A rule built from a lambda |
| `AndSpecification<T>`, `OrSpecification<T>`, `NotSpecification<T>` | What the combinators return. You rarely create them directly |

## Troubleshooting

- **`SpecificationBase<T>` has no `And` method.** Only `CompositeSpecification<T>` has the combinators. Derive your rule from `CompositeSpecification<T>`, not from `SpecificationBase<T>`.
- **Two equal-looking objects are treated as different.** Override `Equals` and `GetHashCode`, or use a `record`.
- **A database query loads every row.** See the note on `FunctionalSpecification<T>` above.

## Samples and tests

`FluentSpecification.Samples` is a runnable console example (loans and customers):

```sh
dotnet run --project FluentSpecification.Samples
```

`FluentSpecification.Tests` holds the unit tests:

```sh
dotnet test
```

## Contributing, security and licence

See [CONTRIBUTING.md](https://github.com/IcodeNet/FluentSpecification/blob/master/CONTRIBUTING.md), [SECURITY.md](https://github.com/IcodeNet/FluentSpecification/blob/master/SECURITY.md) and [CHANGELOG.md](https://github.com/IcodeNet/FluentSpecification/blob/master/CHANGELOG.md). Released under the [MIT licence](https://github.com/IcodeNet/FluentSpecification/blob/master/LICENSE).

## Project structure

```
FluentSpecification/
├── FluentSpecification/          # The NuGet package (netstandard2.0)
├── FluentSpecification.Tests/    # xUnit tests
└── FluentSpecification.Samples/  # Runnable examples (Loan / Customer domain)
```
