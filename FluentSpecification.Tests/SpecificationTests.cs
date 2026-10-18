using System.Linq;
using Xunit;

namespace FluentSpecification.Tests
{
    public class SpecificationTests
    {
        private static readonly int[] Numbers = { 1, 2, 3, 4, 5, 6 };

        private static readonly FunctionalSpecification<int> Even = new FunctionalSpecification<int>(n => n % 2 == 0);
        private static readonly FunctionalSpecification<int> GreaterThanThree = new FunctionalSpecification<int>(n => n > 3);

        [Fact]
        public void IsSatisfiedBy_ReturnsTrue_WhenItemMatchesRule()
        {
            Assert.True(Even.IsSatisfiedBy(4));
        }

        [Fact]
        public void IsSatisfiedBy_ReturnsFalse_WhenItemDoesNotMatchRule()
        {
            Assert.False(Even.IsSatisfiedBy(3));
        }

        [Fact]
        public void SatisfyingElementsFrom_ReturnsOnlyMatchingItems()
        {
            Assert.Equal(new[] { 2, 4, 6 }, Even.SatisfyingElementsFrom(Numbers.AsQueryable()).ToArray());
        }

        [Fact]
        public void SatisfyingElementsFrom_ReturnsEmpty_WhenNothingMatches()
        {
            var none = new FunctionalSpecification<int>(n => n > 100);

            Assert.Empty(none.SatisfyingElementsFrom(Numbers.AsQueryable()));
        }

        [Fact]
        public void And_ReturnsItemsMatchingBothRules()
        {
            Assert.Equal(new[] { 4, 6 }, Even.And(GreaterThanThree).SatisfyingElementsFrom(Numbers.AsQueryable()).OrderBy(n => n).ToArray());
        }

        [Fact]
        public void Or_ReturnsItemsMatchingEitherRuleWithoutDuplicates()
        {
            Assert.Equal(new[] { 2, 4, 5, 6 }, Even.Or(GreaterThanThree).SatisfyingElementsFrom(Numbers.AsQueryable()).OrderBy(n => n).ToArray());
        }

        [Fact]
        public void Not_ReturnsItemsThatDoNotMatchTheRule()
        {
            Assert.Equal(new[] { 1, 3, 5 }, Even.Not().SatisfyingElementsFrom(Numbers.AsQueryable()).OrderBy(n => n).ToArray());
        }

        [Fact]
        public void And_CanBeChainedWithNot()
        {
            var oddAndGreaterThanThree = GreaterThanThree.And(Even.Not());

            Assert.Equal(new[] { 5 }, oddAndGreaterThanThree.SatisfyingElementsFrom(Numbers.AsQueryable()).ToArray());
        }

        [Fact]
        public void Combinators_CanBeChainedIndefinitely()
        {
            var chained = Even.And(GreaterThanThree).Or(Even.Not()).Not();

            Assert.Equal(new[] { 2 }, chained.SatisfyingElementsFrom(Numbers.AsQueryable()).ToArray());
        }

        [Fact]
        public void IsSatisfiedBy_WorksOnCombinedRules()
        {
            Assert.False(Even.And(GreaterThanThree).IsSatisfiedBy(2));
        }

        [Fact]
        public void SatisfyingElementsFrom_KeepsDuplicates_ForASingleRule()
        {
            Assert.Equal(new[] { 2, 2 }, Even.SatisfyingElementsFrom(new[] { 1, 2, 2 }.AsQueryable()).ToArray());
        }

        [Fact]
        public void Combinators_RemoveDuplicates_BecauseTheyUseSetOperations()
        {
            var duplicates = new[] { 2, 2, 4 }.AsQueryable();

            Assert.Equal(new[] { 2, 4 }, Even.And(Even).SatisfyingElementsFrom(duplicates).OrderBy(n => n).ToArray());
            Assert.Equal(new[] { 2, 4 }, Even.Or(Even).SatisfyingElementsFrom(duplicates).OrderBy(n => n).ToArray());
        }

        [Fact]
        public void And_ComparesByReference_WhenTheTypeHasNoValueEquality()
        {
            var first = new Item { Id = 1 };
            var secondWithSameId = new Item { Id = 1 };
            var items = new[] { first, secondWithSameId }.AsQueryable();
            var anyItem = new FunctionalSpecification<Item>(i => true);

            Assert.Equal(2, anyItem.And(anyItem).SatisfyingElementsFrom(items).Count());
        }

        [Fact]
        public void ExpressionSpecification_FiltersInMemory_WhenGivenALocalSet()
        {
            var even = new ExpressionSpecification<int>(n => n % 2 == 0);

            Assert.Equal(new[] { 2, 4, 6 }, even.SatisfyingElementsFrom(Numbers.AsQueryable()).ToArray());
            Assert.True(even.IsSatisfiedBy(4));
        }

        [Fact]
        public void Not_RemovesDuplicates_BecauseItUsesASetOperation()
        {
            var duplicates = new[] { 1, 1, 2 }.AsQueryable();
            var greaterThanFive = new ExpressionSpecification<int>(n => n > 5);

            Assert.Equal(new[] { 1, 2 }, greaterThanFive.Not().SatisfyingElementsFrom(duplicates).OrderBy(n => n).ToArray());
        }

        [Theory]
        [InlineData(2, true)]
        [InlineData(5, false)]
        [InlineData(6, false)]
        [InlineData(7, false)]
        public void IsSatisfiedBy_MatchesTheQueryResult_ForEveryCombinator(int value, bool expected)
        {
            var combined = Even.Or(GreaterThanThree).And(GreaterThanThree.Not());

            Assert.Equal(expected, combined.IsSatisfiedBy(value));
            Assert.Equal(expected, combined.SatisfyingElementsFrom(new[] { value }.AsQueryable()).Any());
        }

        [Fact]
        public void IsSatisfiedBy_UsesTheOverriddenQuery_ForACustomSpecification()
        {
            var custom = new EvenOnly();

            Assert.True(custom.IsSatisfiedBy(4));
            Assert.False(custom.IsSatisfiedBy(3));
        }

        [Fact]
        public void IsSatisfiedBy_CanBeCalledRepeatedly_OnAnExpressionSpecification()
        {
            var even = new ExpressionSpecification<int>(n => n % 2 == 0);

            Assert.True(even.IsSatisfiedBy(2));
            Assert.False(even.IsSatisfiedBy(3));
            Assert.True(even.IsSatisfiedBy(4));
        }

        private class EvenOnly : CompositeSpecification<int>
        {
            public override IQueryable<int> SatisfyingElementsFrom(IQueryable<int> candidates)
            {
                return candidates.Where(n => n % 2 == 0);
            }
        }

        private class Item
        {
            public int Id { get; set; }
        }
    }
}
