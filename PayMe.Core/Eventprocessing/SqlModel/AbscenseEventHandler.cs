using PayMe.Core.Entities;
using PayMe.Core.Events;
using servus.core.Entities;
using System.Collections.Generic;

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
                From = ev.StartDate,
                To = ev.EndDate,
                InstanceId = ev.InstanceId.Value,
                UserId = ev.UserId
            });
        }

        public void Edit(EditAbscenseEvent ev)
        {
            var abscense = db.Abscenses.Find(ev.AbscenseId);
            abscense.From = ev.StartDate;
            abscense.To = ev.EndDate;
        }

        public void Delete(DeleteAbscenseEvent ev)
        {
            var abscense = db.Abscenses.Find(ev.AbscenseId);
            db.Abscenses.Remove(abscense);
        }
    }
}
