using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification.Samples
{

    public class LoanRepository
    {
        public IQueryable<Loan> FindElementsBy(SpecificationBase<Loan> spec)
        {
            Loan[] loans = new[]{
                                new Loan(){DateTaken = DateTime.Now.AddDays(-15) , LoanPeriod = new TimeSpan(14,0,0,0),Amount = 15000.0},
                                new Loan(){DateTaken = DateTime.Now.AddDays(-5), LoanPeriod = new TimeSpan(6,0,0,0),Amount = 10000.0},
                                new Loan(){DateTaken = DateTime.Now.AddDays(-7), LoanPeriod = new TimeSpan(6,0,0,0),Amount = 5000.0}
                                };



            return spec.SatisfyingElementsFrom(loans.AsQueryable());
        }
    }
}
