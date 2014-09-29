using PayMe.Core.Entities;
using PayMe.Core.Events;
using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PayMe.Core.Eventprocessing.SqlModel
{
    public class AbscenseEventHandler : EventHandlerBase
    {
        private Context db;
        public AbscenseEventHandler(Context db)
        {
            this.db = db;
            Handles<CreateAbscenseEvent>(Create);
            Handles<EditAbscenseEvent>(Edit);
            Handles<DeleteAbscenseEvent>(Delete);
        }

        public void Create(CreateAbscenseEvent ev)
        {
            db.Abscenses.Add(new Abscense
            {
                Id = ev.AbscenseId,
                From = ev.StartDate,
                To = ev.EndDate,
                InstanceId = ev.InstanceId.Value,
                UserId = ev.UserId,
                Description = ev.Description
            });
        }

        public void Edit(EditAbscenseEvent ev)
        {
            var abscense = db.Abscenses.First(a => a.InstanceId == ev.InstanceId.Value && a.Id == ev.AbscenseId);
            if (abscense.UserId != ev.UserId)
            {
                throw new InvalidOperationException("user was not owner of the abscense");
            }
            abscense.From = ev.StartDate;
            abscense.To = ev.EndDate;
            abscense.Description = ev.Description;
        }

        public void Delete(DeleteAbscenseEvent ev)
        {
            var abscense = db.Abscenses.Find(ev.AbscenseId);
            db.Abscenses.Remove(abscense);
        }
    }
}
