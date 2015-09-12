using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification
{
    public abstract class SpecificationBase<T>
    {
        public bool IsSatisfiedBy(T candidate)
        {
            return SatisfyingElementsFrom((new[] { candidate }).AsQueryable()).Any();
        }

        public abstract IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates);

    }
}
