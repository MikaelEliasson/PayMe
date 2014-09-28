using PayMe.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Eventprocessing
{
    public class EventHandlerBase
    {
        public Dictionary<Type, Action<EventBase>> SupportedEvents { get; set; }

        public EventHandlerBase()
        {
            SupportedEvents = new Dictionary<Type, Action<EventBase>>();
        }

        public void Handles<T>(Action<T> handler) where T : EventBase
        {
            this.SupportedEvents.Add(typeof(T), ev => handler((T)ev));
        }

        public bool Handle<T>(T ev) where T : EventBase
        {
            if (SupportedEvents.ContainsKey(typeof(T)))
            {
                SupportedEvents[typeof(T)](ev);
                return true;
            }
            return false;
        }
    }
}
