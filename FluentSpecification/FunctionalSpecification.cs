using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification
{

    public class FunctionalSpecification<T> : CompositeSpecification<T>
    {
        private Func<T, bool> predicate;

        public FunctionalSpecification(Func<T, bool> condition)
        {
            this.predicate = condition;
        }


        public override IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates)
        {
            return candidates.Where(c => predicate(c)).Select(c => c);
        }


    }

}
