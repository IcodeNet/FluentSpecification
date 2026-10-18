using System;
using System.Linq;

namespace FluentSpecification
{
    /// <summary>
    /// A specification built from a lambda, so a simple rule needs no new class.
    /// </summary>
    /// <remarks>
    /// The lambda is a <see cref="Func{T, TResult}"/>, not an expression tree. It works on in-memory data.
    /// Query providers such as Entity Framework cannot translate it to SQL, so they run it in memory after loading the rows.
    /// </remarks>
    /// <typeparam name="T">The type of item the rule applies to.</typeparam>
    public class FunctionalSpecification<T> : CompositeSpecification<T>
    {
        private readonly Func<T, bool> predicate;

        /// <summary>
        /// Creates a specification from a rule.
        /// </summary>
        /// <param name="condition">Returns <c>true</c> for items that satisfy the rule.</param>
        public FunctionalSpecification(Func<T, bool> condition)
        {
            predicate = condition;
        }

        /// <inheritdoc />
        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            return candidates.Where(c => predicate(c)).Select(c => c);
        }
    }
}
