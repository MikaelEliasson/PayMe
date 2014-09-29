using PayMe.Core.Entities;
using PayMe.Core.Events;
using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PayMe.Core.Eventprocessing.SqlModel
{
    public class TransferEventHandler : EventHandlerBase
    {
        private Context db;
        public TransferEventHandler(Context db)
        {
            this.db = db;
            Handles<CreateTransferEvent>(Create);
            Handles<EditTransferEvent>(Edit);
            Handles<DeleteTransferEvent>(Delete);
        }

        public void Create(CreateTransferEvent ev)
        {
            db.Transfers.Add(new Transfer
            {
                Id = ev.TransferId,
                FromUserId = ev.UserId,
                ToUserId = ev.ToUserId,
                Sum  = ev.Sum,
                Date = ev.Date,
                InstanceId = ev.InstanceId.Value
            });
        }

        public void Edit(EditTransferEvent ev)
        {
            var transfer = db.Transfers.First(a => a.InstanceId == ev.InstanceId.Value && a.Id == ev.TransferId);
            if (transfer.FromUserId != ev.UserId)
            {
                throw new InvalidOperationException("user was not owner of the abscense");
            }
            transfer.ToUserId = ev.ToUserId;
            transfer.Sum  = ev.Sum;
            transfer.Date = ev.Date;
        }

        public void Delete(DeleteTransferEvent ev)
        {
            var transfer = db.Transfers.Find(ev.TransferId);
            if (transfer.FromUserId != ev.UserId)
            {
                throw new InvalidOperationException();
            }
            db.Transfers.Remove(transfer);
        }
    }
}
