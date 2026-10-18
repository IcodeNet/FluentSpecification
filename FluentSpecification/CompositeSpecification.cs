using System;

namespace FluentSpecification
{
    /// <summary>
    /// A specification that can be combined with others using <see cref="And"/>, <see cref="Or"/> and <see cref="Not"/>.
    /// Derive from this class to write your own named rules.
    /// </summary>
    /// <typeparam name="T">The type of item the rule applies to.</typeparam>
    public abstract class CompositeSpecification<T> : SpecificationBase<T>
    {
        /// <summary>
        /// Builds a specification that is satisfied only when both this one and <paramref name="otherCandidate"/> are satisfied.
        /// </summary>
        /// <remarks>
        /// Implemented with <c>Intersect</c>. In memory, items are compared with <c>Equals</c> and <c>GetHashCode</c>
        /// (types without value equality are compared by reference). On a database, the provider compares whole rows instead.
        /// The order of the result is not guaranteed.
        /// </remarks>
        /// <param name="otherCandidate">The second rule.</param>
        /// <exception cref="ArgumentNullException"><paramref name="otherCandidate"/> is <c>null</c>.</exception>
        public CompositeSpecification<T> And(SpecificationBase<T> otherCandidate)
        {
            if (otherCandidate == null) throw new ArgumentNullException(nameof(otherCandidate));
            return new AndSpecification<T>(this, otherCandidate);
        }

        /// <summary>
        /// Builds a specification that is satisfied when either this one or <paramref name="otherCandidate"/> is satisfied.
        /// </summary>
        /// <remarks>
        /// Implemented with <c>Union</c>, so the result has no duplicates. In memory, items are compared with <c>Equals</c>
        /// and <c>GetHashCode</c>. On a database, the provider compares whole rows instead. The order of the result is not guaranteed.
        /// </remarks>
        /// <param name="otherCandidate">The second rule.</param>
        /// <exception cref="ArgumentNullException"><paramref name="otherCandidate"/> is <c>null</c>.</exception>
        public CompositeSpecification<T> Or(SpecificationBase<T> otherCandidate)
        {
            if (otherCandidate == null) throw new ArgumentNullException(nameof(otherCandidate));
            return new OrSpecification<T>(this, otherCandidate);
        }

        /// <summary>
        /// Builds a specification that is satisfied when this one is not.
        /// </summary>
        /// <remarks>
        /// Implemented with <c>Except</c>. In memory, items are compared with <c>Equals</c> and <c>GetHashCode</c>.
        /// On a database, the provider compares whole rows instead. The order of the result is not guaranteed.
        /// </remarks>
        public CompositeSpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
