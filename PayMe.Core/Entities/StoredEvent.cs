using Newtonsoft.Json;
using PayMe.Core.Events;
using System;

namespace PayMe.Core.Entities
{
    public class StoredEvent
    {
        public long Id { get; set; }
        public Guid? InstanceId { get; set; }
        public DateTime TimeUtc { get; set; }
        public string EventType { get; set; }
        public Guid UserId { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
        public string EventAsJson { get; set; }

        public static StoredEvent FromEvent<T>(T eventToStore) where T : EventBase
        {
            return new StoredEvent
            {
                InstanceId = eventToStore.InstanceId,
                EventType = eventToStore.EventType,
                Ip = eventToStore.Ip,
                TimeUtc = eventToStore.TimeUtc,
                UserAgent = eventToStore.UserAgent,
                UserId = eventToStore.UserId,
                EventAsJson = JsonConvert.SerializeObject(eventToStore)
            };
        }

        public T ToEvent<T>() where T : EventBase
        {
            if (typeof(T).Name != EventType)
            {
                throw new ArgumentException("The event was of the type " + EventType + " and cannot be deserialized to a <" + typeof(T).Name + ">");
            }
            return JsonConvert.DeserializeObject<T>(EventAsJson);
        }
    }
}
