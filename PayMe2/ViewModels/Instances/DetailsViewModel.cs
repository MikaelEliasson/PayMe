using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Instances
{
    public class DetailsViewModel
    {
        public Instance Instance { get; set; }
        public List<EventForList> LastChanges { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public string GetName(Guid userId)
        {
            var user = Users.First(u => u.Id == userId);
            return user.FirstName + " " + user.LastName;
        }

        public string JoinUrl { get; set; }

        public IEnumerable<PayMe.Core.Calculations.UserBalance> UserBalances { get; set; }

        public IEnumerable<Category> Categories
        {
            get
            {
                return UserBalances.SelectMany(ub => ub.ExpensesPerCategory.Select(e => e.Key)).Distinct();
            }
        }
        public decimal CalculateTotalExpense(Category category)
        {
            decimal TotalExpense = 0;
            foreach (var userbalance in UserBalances)
            {
                if (userbalance.ExpensesPerCategory.ContainsKey(category))
                {
                    TotalExpense += userbalance.ExpensesPerCategory[category];
                }
            }
            return TotalExpense;
        }
        public decimal CalculateTotalExpensePerDay(Category category)
        {
            decimal TotalExpense = CalculateTotalExpense(category);
            int TotalDays = UserBalances.Sum(ub => ub.DaysPresent);
            return TotalExpense / TotalDays;
        }
    }
}