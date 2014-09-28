using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Events
{
    public abstract class EventBase
    {
        public Guid? InstanceId { get; set; }
        public DateTime TimeUtc { get; set; }
        public string EventType { get; set; }

        public Guid UserId { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
    }

    public static class EventBaseExtensions
    {
        public static T FillBase<T>(this T source, AuditInfo audit, Guid? instanceId) where T : EventBase
        {
            source.EventType = typeof(T).Name;
            source.Ip = audit.Ip;
            source.TimeUtc = DateTime.UtcNow;
            source.UserAgent = audit.UserAgent;
            source.UserId = audit.UserId;
            source.InstanceId = instanceId;
            return source;
        }
    }
}
