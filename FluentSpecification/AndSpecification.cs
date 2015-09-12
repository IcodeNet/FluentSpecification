using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification
{


    public class AndSpecification<T> : CompositeSpecification<T>
    {
        private SpecificationBase<T> one;
        private SpecificationBase<T> two;

        public AndSpecification(SpecificationBase<T> x, SpecificationBase<T> y)
        {
            one = x;
            two = y;
        }


        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            return (one.SatisfyingElementsFrom(candidates)).Intersect(two.SatisfyingElementsFrom(candidates));

        }

    }


}
