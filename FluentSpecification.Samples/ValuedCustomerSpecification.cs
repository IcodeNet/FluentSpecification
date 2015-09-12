using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification.Samples
{
    public class ValuedCustomerSpecification : SpecificationBase<Customer>
    {
      public override IQueryable<Customer> SatisfyingElementsFrom(IQueryable<Customer> candidates)
        {
            return candidates.Where(c => c.Balance > 1300d).Select(c => c);
        }
    }

}
