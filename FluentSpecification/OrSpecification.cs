using System.Linq;

namespace FluentSpecification
{
    /// <summary>
    /// Satisfied by items that satisfy either of two specifications. Create it with <see cref="CompositeSpecification{T}.Or"/>.
    /// </summary>
    /// <typeparam name="T">The type of item the rule applies to.</typeparam>
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        private readonly SpecificationBase<T> one;
        private readonly SpecificationBase<T> two;

        /// <summary>
        /// Creates a specification from two rules where at least one must be satisfied.
        /// </summary>
        /// <param name="x">The first rule.</param>
        /// <param name="y">The second rule.</param>
        public OrSpecification(SpecificationBase<T> x, SpecificationBase<T> y)
        {
            one = x;
            two = y;
        }

        /// <inheritdoc />
        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            return one.SatisfyingElementsFrom(candidates).Union(two.SatisfyingElementsFrom(candidates));
        }
    }
}
