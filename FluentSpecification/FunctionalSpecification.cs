using System;
using System.Linq;

namespace FluentSpecification
{
    /// <summary>
    /// A specification built from a lambda, so a simple rule needs no new class.
    /// </summary>
    /// <remarks>
    /// The lambda is a <see cref="Func{T, TResult}"/>, not an expression tree. It works on in-memory data only.
    /// Query providers such as Entity Framework cannot translate it to SQL and throw <see cref="InvalidOperationException"/>
    /// when you enumerate the query. For database queries use <see cref="ExpressionSpecification{T}"/>,
    /// or load the rows first with <c>ToList().AsQueryable()</c>.
    /// </remarks>
    /// <typeparam name="T">The type of item the rule applies to.</typeparam>
    public class FunctionalSpecification<T> : CompositeSpecification<T>
    {
        private readonly Func<T, bool> predicate;

        /// <summary>
        /// Creates a specification from a rule.
        /// </summary>
        /// <param name="condition">Returns <c>true</c> for items that satisfy the rule.</param>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is <c>null</c>.</exception>
        public FunctionalSpecification(Func<T, bool> condition)
        {
            predicate = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        /// <inheritdoc />
        public override bool IsSatisfiedBy(T candidate)
        {
            return predicate(candidate);
        }

        /// <inheritdoc />
        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            if (candidates == null) throw new ArgumentNullException(nameof(candidates));
            return candidates.Where(c => predicate(c));
        }
    }
}
