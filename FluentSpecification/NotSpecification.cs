using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification
{

    public class NotSpecification<T> : SpecificationBase<T>
    {

        private SpecificationBase<T> wrapped;

        public NotSpecification(SpecificationBase<T> x)
        {
            wrapped = x;

        }


        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            return (candidates.Except(wrapped.SatisfyingElementsFrom(candidates)));

        }

    }


}
