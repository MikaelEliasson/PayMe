using PayMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PayMe2.ViewModels
{
    public class EventForList
    {
        public static Expression<Func<StoredEvent, EventForList>> Transform
        {
            get
            {
                return e => new EventForList
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    TimeUtc = e.TimeUtc,
                    EventType = e.EventType
                };
            }
        }

        public long Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime TimeUtc { get; set; }

        public string EventType { get; set; }
    }
}