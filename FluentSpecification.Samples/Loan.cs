using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification.Samples
{
    public class Loan
    {

        public string Holder { get; set; }
        public double Amount { get; set; }
        public DateTime DateTaken { get; set; }
        public TimeSpan LoanPeriod { get; set; }


        public bool IsOverDue()
        {
            return DateTaken.Add(LoanPeriod) > DateTime.Now;
        }
    }
}
