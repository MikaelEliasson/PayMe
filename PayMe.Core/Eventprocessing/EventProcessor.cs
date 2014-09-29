using PayMe.Core.Eventprocessing.SqlModel;
using PayMe.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Eventprocessing
{
    public static class EventProcessor
    {
        private static IEnumerable<EventHandlerBase> GetHandlers(Context db)
        {
            yield return new InstanceEventHandler(db);
            yield return new AbscenseEventHandler(db);
            yield return new CategoryEventHandler(db);
            yield return new ExpenseEventHandler(db);
            yield return new TransferEventHandler(db);
        }
        public static void Process<T>(Context db, T ev) where T : EventBase
        {
            var handlers = GetHandlers(db);
            foreach (var item in handlers)
            {
                item.Handle(ev);
            }
        }
    }
}
