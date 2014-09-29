using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Events
{
    public class CreateTransferEvent : EventBase
    {
        public Guid TransferId { get; set; }
        public Guid ToUserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
    }

    public class EditTransferEvent : CreateTransferEvent
    {
    }

    public class DeleteTransferEvent : EventBase
    {
        public Guid TransferId { get; set; }
    }

    public static class TransferEventFactory
    {
        public static CreateTransferEvent CreateTransfer(Guid instanceId, Guid transferId, Guid toId, decimal sum, DateTime date, AuditInfo audit)
        {
            return new CreateTransferEvent
            {
                TransferId = transferId,
                ToUserId = toId,
                Date = date,
                Sum = sum,
            }.FillBase(audit, instanceId);
        }

        public static EditTransferEvent EditTransfer(Guid instanceId, Guid transferId, Guid toId, decimal sum, DateTime date, AuditInfo audit)
        {
            return new EditTransferEvent
            {
                TransferId = transferId,
                ToUserId = toId,
                Date = date,
                Sum = sum,
            }.FillBase(audit, instanceId);
        }

        public static DeleteTransferEvent DeleteExpense(Guid instanceId, Guid transferId, AuditInfo audit)
        {
            return new DeleteTransferEvent
            {
                TransferId = transferId,
            }.FillBase(audit, instanceId);
        }
    }
}
