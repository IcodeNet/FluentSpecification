using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification
{

    public class OrSpecification<T> : SpecificationBase<T>
    {
        private SpecificationBase<T> one;
        private SpecificationBase<T> two;

        public OrSpecification(SpecificationBase<T> x, SpecificationBase<T> y)
        {
            one = x;
            two = y;
        }


        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            return (one.SatisfyingElementsFrom(candidates)).Union(two.SatisfyingElementsFrom(candidates));

        }

    }


}
