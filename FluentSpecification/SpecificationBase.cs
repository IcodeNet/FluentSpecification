using System.Linq;

namespace FluentSpecification
{
    /// <summary>
    /// The root of every specification: a business rule that selects the items it accepts from a set of candidates.
    /// </summary>
    /// <typeparam name="T">The type of item the rule applies to.</typeparam>
    public abstract class SpecificationBase<T>
    {
        /// <summary>
        /// Checks whether a single item satisfies this specification.
        /// </summary>
        /// <param name="candidate">The item to check.</param>
        /// <returns><c>true</c> if the item satisfies the rule; otherwise <c>false</c>.</returns>
        public bool IsSatisfiedBy(T candidate)
        {
            return SatisfyingElementsFrom((new[] { candidate }).AsQueryable()).Any();
        }

        /// <summary>
        /// Filters a set of candidates down to the items that satisfy this specification.
        /// </summary>
        /// <param name="candidates">The items to filter.</param>
        /// <returns>The candidates that satisfy the rule.</returns>
        public abstract IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates);
    }
}
