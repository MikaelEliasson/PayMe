using System;

namespace PayMe.Core.Events
{
    public class CreateExpenseEvent : EventBase
    {
        public Guid ExpenseId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid[] AffectedUsers { get; set; }
        public string Shop { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
    }

    public class EditExpenseEvent : CreateExpenseEvent
    {

    }

    public class DeleteExpenseEvent : EventBase
    {
        public Guid ExpenseId { get; set; }
    }

    public static class ExpenseEventFactory
    {
        public static CreateExpenseEvent CreateExpense(Guid instanceId, Guid expenseId, Guid categoryId, string shop, decimal sum, DateTime date, Guid[] affectedUsers, AuditInfo audit)
        {
            return new CreateExpenseEvent
            {
                ExpenseId = expenseId,
                CategoryId = categoryId,
                Date = date,
                Shop = shop,
                Sum = sum,
                AffectedUsers = affectedUsers
            }.FillBase(audit, instanceId);
        }

        public static EditExpenseEvent EditExpense(Guid instanceId, Guid expenseId, Guid categoryId, string shop, decimal sum, DateTime date, Guid[] affectedUsers, AuditInfo audit)
        {
            return new EditExpenseEvent
            {
                Date = date,
                ExpenseId = expenseId,
                CategoryId = categoryId,
                Shop = shop,
                Sum = sum,
                AffectedUsers = affectedUsers
            }.FillBase(audit, instanceId);
        }

        public static DeleteExpenseEvent DeleteExpense(Guid instanceId, Guid expenseId, AuditInfo audit)
        {
            return new DeleteExpenseEvent
            {
                ExpenseId = expenseId,
            }.FillBase(audit, instanceId);
        }
    }
}
