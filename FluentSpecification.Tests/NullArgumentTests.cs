using System;
using System.Linq;
using Xunit;

namespace FluentSpecification.Tests
{
    public class NullArgumentTests
    {
        private static readonly FunctionalSpecification<int> Any = new FunctionalSpecification<int>(n => true);

        [Fact]
        public void FunctionalSpecification_Throws_WhenConditionIsNull()
        {
            var error = Assert.Throws<ArgumentNullException>(() => new FunctionalSpecification<int>(null!));

            Assert.Equal("condition", error.ParamName);
        }

        [Fact]
        public void ExpressionSpecification_Throws_WhenConditionIsNull()
        {
            var error = Assert.Throws<ArgumentNullException>(() => new ExpressionSpecification<int>(null!));

            Assert.Equal("condition", error.ParamName);
        }

        [Fact]
        public void And_Throws_WhenOtherRuleIsNull()
        {
            var error = Assert.Throws<ArgumentNullException>(() => Any.And(null!));

            Assert.Equal("otherCandidate", error.ParamName);
        }

        [Fact]
        public void Or_Throws_WhenOtherRuleIsNull()
        {
            var error = Assert.Throws<ArgumentNullException>(() => Any.Or(null!));

            Assert.Equal("otherCandidate", error.ParamName);
        }

        [Fact]
        public void NotSpecification_Throws_WhenWrappedRuleIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NotSpecification<int>(null!));
        }

        [Fact]
        public void SatisfyingElementsFrom_Throws_WhenCandidatesAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => Any.SatisfyingElementsFrom(null!));
            Assert.Throws<ArgumentNullException>(() => new ExpressionSpecification<int>(n => true).SatisfyingElementsFrom(null!));
        }
    }
}
