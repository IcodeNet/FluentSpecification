using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification.Samples
{

    public class OverDueLoanSpecification : CompositeSpecification<Loan>
    {
        public override IQueryable<Loan> SatisfyingElementsFrom(IQueryable<Loan> candidates)
        {
            return from l in candidates
                   where l.DateTaken < DateTime.Today
                   where l.DateTaken.Add(l.LoanPeriod) < DateTime.Now
                   select l;

        }
    }
}
