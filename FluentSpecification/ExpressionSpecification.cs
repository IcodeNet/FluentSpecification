using System;
using System.Linq;
using System.Linq.Expressions;

namespace FluentSpecification
{
    /// <summary>
    /// A specification built from an expression tree, so query providers such as Entity Framework can translate it to SQL.
    /// </summary>
    /// <remarks>
    /// Prefer this over <see cref="FunctionalSpecification{T}"/> when the data comes from a database.
    /// The expression may only use constructs the provider understands.
    /// </remarks>
    /// <typeparam name="T">The type of item the rule applies to.</typeparam>
    public class ExpressionSpecification<T> : CompositeSpecification<T>
    {
        private readonly Expression<Func<T, bool>> predicate;
        private readonly Lazy<Func<T, bool>> compiled;

        /// <summary>
        /// Creates a specification from a rule written as an expression.
        /// </summary>
        /// <param name="condition">The rule, for example <c>l =&gt; l.Amount &gt; 10000</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is <c>null</c>.</exception>
        public ExpressionSpecification(Expression<Func<T, bool>> condition)
        {
            predicate = condition ?? throw new ArgumentNullException(nameof(condition));
            compiled = new Lazy<Func<T, bool>>(predicate.Compile);
        }

        /// <inheritdoc />
        public override bool IsSatisfiedBy(T candidate)
        {
            return compiled.Value(candidate);
        }

        /// <inheritdoc />
        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            if (candidates == null) throw new ArgumentNullException(nameof(candidates));
            return candidates.Where(predicate);
        }
    }
}
