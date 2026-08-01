# FluentSpecification

A lightweight C# implementation of the **Specification pattern** that lets you encapsulate business rules as reusable, composable objects and combine them fluently with `And`, `Or`, and `Not`.

---

## Why?

Instead of scattering `if` conditions and LINQ predicates across your codebase, you express each rule as a named class (or a one-liner lambda). Rules stay decoupled from your domain objects and can be composed without touching either.

---

## Core Classes

| Class | Purpose |
|---|---|
| `SpecificationBase<T>` | Abstract base – implement `SatisfyingElementsFrom` to define your rule |
| `CompositeSpecification<T>` | Extends `SpecificationBase<T>` with `And`, `Or`, and `Not` combinators |
| `FunctionalSpecification<T>` | Ready-made specification built from a lambda – no subclass needed |

---

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

---

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

---

## Project Structure

```
FluentSpecification/
├── FluentSpecification/          # Core library
│   ├── SpecificationBase.cs
│   ├── CompositeSpecification.cs
│   ├── AndSpecification.cs
│   ├── OrSpecification.cs
│   ├── NotSpecification.cs
│   └── FunctionalSpecification.cs
└── FluentSpecification.Samples/  # Runnable examples (Loan / Customer domain)
```
