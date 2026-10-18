using System;
using System.Linq;

namespace FluentSpecification
{
    /// <summary>
    /// Satisfied by items that do not satisfy the wrapped specification. Create it with <see cref="CompositeSpecification{T}.Not"/>.
    /// </summary>
    /// <typeparam name="T">The type of item the rule applies to.</typeparam>
    public class NotSpecification<T> : CompositeSpecification<T>
    {
        private readonly SpecificationBase<T> wrapped;

        /// <summary>
        /// Creates a specification that reverses another rule.
        /// </summary>
        /// <param name="x">The rule to reverse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="x"/> is <c>null</c>.</exception>
        public NotSpecification(SpecificationBase<T> x)
        {
            wrapped = x ?? throw new ArgumentNullException(nameof(x));
        }

        /// <inheritdoc />
        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            return candidates.Except(wrapped.SatisfyingElementsFrom(candidates));
        }
    }
}
