using System;
using System.Linq;

namespace FluentSpecification
{
    /// <summary>
    /// Satisfied only by items that satisfy both of two specifications. Create it with <see cref="CompositeSpecification{T}.And"/>.
    /// </summary>
    /// <typeparam name="T">The type of item the rule applies to.</typeparam>
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        private readonly SpecificationBase<T> one;
        private readonly SpecificationBase<T> two;

        /// <summary>
        /// Creates a specification from two rules that must both be satisfied.
        /// </summary>
        /// <param name="x">The first rule.</param>
        /// <param name="y">The second rule.</param>
        /// <exception cref="ArgumentNullException"><paramref name="x"/> or <paramref name="y"/> is <c>null</c>.</exception>
        public AndSpecification(SpecificationBase<T> x, SpecificationBase<T> y)
        {
            one = x ?? throw new ArgumentNullException(nameof(x));
            two = y ?? throw new ArgumentNullException(nameof(y));
        }

        /// <inheritdoc />
        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            return one.SatisfyingElementsFrom(candidates).Intersect(two.SatisfyingElementsFrom(candidates));
        }
    }
}
