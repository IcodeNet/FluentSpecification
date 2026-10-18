using System;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FluentSpecification.Tests
{
    public class DatabaseTests : IDisposable
    {
        private readonly SqliteConnection connection = new SqliteConnection("Data Source=:memory:");
        private readonly LoanContext db;

        public DatabaseTests()
        {
            connection.Open();
            db = new LoanContext(connection);
            db.Database.EnsureCreated();
            db.Loans.AddRange(new Loan { Amount = 5 }, new Loan { Amount = 20 }, new Loan { Amount = 30 });
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
            connection.Dispose();
        }

        [Fact]
        public void ExpressionSpecification_RunsAsSql_WhenUsedWithEntityFramework()
        {
            var big = new ExpressionSpecification<Loan>(l => l.Amount > 10);

            var query = big.SatisfyingElementsFrom(db.Loans);

            Assert.Contains("WHERE", query.ToQueryString());
            Assert.Equal(2, query.Count());
        }

        [Fact]
        public void FunctionalSpecification_Throws_WhenUsedDirectlyOnADatabaseQuery()
        {
            var big = new FunctionalSpecification<Loan>(l => l.Amount > 10);

            Assert.Throws<InvalidOperationException>(() => big.SatisfyingElementsFrom(db.Loans).ToList());
        }

        [Fact]
        public void FunctionalSpecification_Works_WhenRowsAreLoadedFirst()
        {
            var big = new FunctionalSpecification<Loan>(l => l.Amount > 10);

            Assert.Equal(2, big.SatisfyingElementsFrom(db.Loans.ToList().AsQueryable()).Count());
        }

        [Fact]
        public void Combinators_RunAsSql_WhenEveryRuleIsAnExpression()
        {
            var big = new ExpressionSpecification<Loan>(l => l.Amount > 10);
            var firstTwo = new ExpressionSpecification<Loan>(l => l.Id < 3);

            Assert.Equal(1, big.And(firstTwo).SatisfyingElementsFrom(db.Loans).Count());
            Assert.Equal(3, big.Or(firstTwo).SatisfyingElementsFrom(db.Loans).Count());
            Assert.Equal(1, big.Not().SatisfyingElementsFrom(db.Loans).Count());
        }

        public class Loan
        {
            public int Id { get; set; }
            public int Amount { get; set; }
        }

        private class LoanContext : DbContext
        {
            private readonly SqliteConnection connection;

            public LoanContext(SqliteConnection connection)
            {
                this.connection = connection;
            }

            public DbSet<Loan> Loans => Set<Loan>();

            protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(connection);
        }
    }
}
