using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PayMe.Core.Calculations;
using servus.core.Entities;
using PayMe.Core.Entities;

namespace PayMe2.Core.Calculations
{
    public class ExpenseCalculator
    {
        
        public IEnumerable<UserBalance> CalculateBalances(Instance instance, IEnumerable<ApplicationUser> users, IEnumerable<Expense> expenses, IEnumerable<Transfer> transfers)
        {
            var dayCount = (instance.EndDate ?? DateTime.Today).Subtract(instance.StartDate).Days;
            var balances = users.Select(u => new UserBalance
            {
                User = u,
                ExpensesPerCategory = new Dictionary<Category, decimal>(),
                DaysPresent = dayCount - ((int)u.Abscenses.Sum(a => (a.To - a.From).TotalDays))
            }).ToDictionary(ub => ub.User.Id, ub => ub);

            var totalDays = balances.Sum(b => b.Value.DaysPresent);

            foreach (var expense in expenses)
            {
                balances[expense.OwnerId].TotalAmountPaid += expense.Sum;
                foreach (var userId in expense.AffectedUsers.Split(';').Select(str => Guid.Parse(str)))
                {
                    var expenseMapping = balances[userId].ExpensesPerCategory;
                    var amountToPay = CalculateExpense(expense, totalDays, balances[userId].DaysPresent);
                    if (expenseMapping.ContainsKey(expense.Category))
                    {
                        expenseMapping[expense.Category] += amountToPay;
                    }
                    else
                    {
                        expenseMapping.Add(expense.Category, amountToPay);
                    }
                }
            }

            foreach (var transfer in transfers)
            {
                balances[transfer.ToUserId].TotalAmountPaid -= transfer.Sum;
                balances[transfer.FromUserId].TotalAmountPaid += transfer.Sum;
            }

            return balances.Select(b => b.Value);
        }

        private decimal CalculateExpense(Expense expense, int totalDays, int daysPresent)
        {
            if (expense.Category.Type == CategoryType.SplitEqual)
            {
                return expense.Sum / expense.AffectedUsers.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries).Count();
            }
            else
            {
                return (expense.Sum * daysPresent) / totalDays;
            }
        }
    }
}
