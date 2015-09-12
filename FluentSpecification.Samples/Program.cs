using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentSpecification.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            // The issue that we have here is that Loan should not be coupled to the conditions or expressions that we might have as a requirement to be able to change with out any change to Loan or any clients.
            // Thats where Specifications come in.

            var repo = new LoanRepository();

            var overdueSpec = new OverDueLoanSpecification();
            
            //or

            var funcSpec = new FunctionalSpecification<Loan>(l => l.Amount > 10000.00);

            var overdueLoans = repo.FindElementsBy(overdueSpec);

            var bigLoans = repo.FindElementsBy(funcSpec);

            var overdueAndBigLoans = repo.FindElementsBy(overdueSpec.And(funcSpec));

            var sixDaysDurationSpec = new FunctionalSpecification<Loan>(l => l.LoanPeriod == new TimeSpan(6, 0, 0, 0));

            var fourteenDaysDurationSpec = new FunctionalSpecification<Loan>(l => l.LoanPeriod == new TimeSpan(14, 0, 0, 0));

            var sixOrFourTeenDays = repo.FindElementsBy(sixDaysDurationSpec.Or(fourteenDaysDurationSpec));

            var notSixDaysDurationLoans = repo.FindElementsBy(sixDaysDurationSpec.Not());

            Console.WriteLine("overdueLoans");
            ObjectDumper.Write(overdueLoans);
            Console.WriteLine();

            Console.WriteLine("bigLoans");
            ObjectDumper.Write(bigLoans);
            Console.WriteLine();

            Console.WriteLine("overdueAndBigLoans");
            ObjectDumper.Write(overdueAndBigLoans);
            Console.WriteLine();

            Console.WriteLine("sixOrFourTeenDays");
            ObjectDumper.Write(sixOrFourTeenDays);
            Console.WriteLine();

            Console.WriteLine("notSixDaysDurationLoans");
            ObjectDumper.Write(notSixDaysDurationLoans);
            Console.WriteLine();


            var byron = new Customer() { FirstName = "Byron", Balance = 1000.00 };
           
            var spec = new ValuedCustomerSpecification();

            Console.WriteLine("Is Customer a valued customer ? " + spec.IsSatisfiedBy(byron));


            Console.ReadLine();
        }
    }
}
