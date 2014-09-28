using PayMe.Core.Events;
using servus.core.Entities;
using System.Linq;

namespace PayMe.Core.Eventprocessing.SqlModel
{
    public class ExpenseEventHandler : EventHandlerBase
    {
        private Context db;
        public ExpenseEventHandler(Context db)
        {
            this.db = db;
            Handles<CreateExpenseEvent>(Create);
            Handles<EditExpenseEvent>(Edit);
            Handles<DeleteExpenseEvent>(Delete);
        }

        public void Create(CreateExpenseEvent ev)
        {
            db.Expenses.Add(new Expense
            {
                Id = ev.ExpenseId,
                CategoryId = ev.CategoryId,
                Shop = ev.Shop,
                Sum = ev.Sum,
                Date = ev.Date,
                Created = ev.TimeUtc,
                AffectedUsers = string.Join(";", ev.AffectedUsers),
                OwnerId = ev.UserId,
                InstanceId = ev.InstanceId.Value,
            });
        }

        public void Edit(EditExpenseEvent ev)
        {
            var expense = db.Expenses.Find(ev.ExpenseId);
            expense.CategoryId = ev.CategoryId;
            expense.Shop = ev.Shop;
            expense.Sum = ev.Sum;
            expense.Date = ev.Date;
            expense.Created = ev.TimeUtc;
            expense.AffectedUsers = string.Join(";", ev.AffectedUsers);
        }

        public void Delete(DeleteExpenseEvent ev)
        {
            var expense = db.Expenses.Find(ev.ExpenseId);
            db.Expenses.Remove(expense);
        }
    }
}
