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
        /// Implemented with <c>Intersect</c>, so items are compared with their default equality.
        /// Types without value equality are compared by reference.
        /// </remarks>
        /// <param name="otherCandidate">The second rule.</param>
        public CompositeSpecification<T> And(SpecificationBase<T> otherCandidate)
        {
            return new AndSpecification<T>(this, otherCandidate);
        }

        /// <summary>
        /// Builds a specification that is satisfied when either this one or <paramref name="otherCandidate"/> is satisfied.
        /// </summary>
        /// <remarks>
        /// Implemented with <c>Union</c>, so items are compared with their default equality and the result has no duplicates.
        /// </remarks>
        /// <param name="otherCandidate">The second rule.</param>
        public CompositeSpecification<T> Or(SpecificationBase<T> otherCandidate)
        {
            return new OrSpecification<T>(this, otherCandidate);
        }

        /// <summary>
        /// Builds a specification that is satisfied when this one is not.
        /// </summary>
        /// <remarks>
        /// Implemented with <c>Except</c>, so items are compared with their default equality.
        /// </remarks>
        public CompositeSpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}
