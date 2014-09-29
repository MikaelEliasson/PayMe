using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayMe.Core.Calculations
{
    public class UserBalance
    {
        public ApplicationUser User { get; set; }
        public decimal TotalExpenses
        {
            get
            {
                return ExpensesPerCategory.Sum(kvp => kvp.Value);
            }
        }
        public IDictionary<Category, decimal> ExpensesPerCategory { get; set; }

        public decimal TotalAmountPaid { get; set; }

        public bool HasUnconfirmedTransactions { get; set; }

        public decimal Balance { get { return TotalAmountPaid - TotalExpenses; } }

        public int DaysPresent { get; set; }
    }
}
