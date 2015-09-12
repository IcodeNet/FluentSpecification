using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification
{
    public abstract class CompositeSpecification<T> : SpecificationBase<T>
    {
        public SpecificationBase<T> And(SpecificationBase<T> otherCandidate)
        {
            return new AndSpecification<T>(this, otherCandidate);
        }


        public SpecificationBase<T> Or(SpecificationBase<T> otherCandidate)
        {
            return new OrSpecification<T>(this, otherCandidate);
        }


        public SpecificationBase<T> Not()
        {
            return new NotSpecification<T>(this);
        }

    }

}
