using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Events
{
    public class CreateInstanceEvent : EventBase
    {
        public string Name { get; set; }
        public string JoinCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class EditInstanceEvent : CreateInstanceEvent
    {
    }

    public class JoinInstanceEvent : EventBase
    {
    }

    public static class InstanceEventFactory
    {
        public static CreateInstanceEvent CreateEvent(Guid instanceId, string name, string joinCode, DateTime startDate, DateTime? endDate, AuditInfo audit)
        {
            return new CreateInstanceEvent
            {
                Name = name,
                JoinCode = joinCode,
                StartDate = startDate,
                EndDate = endDate,
            }.FillBase(audit, instanceId);
        }

        public static JoinInstanceEvent JoinEvent(Guid instanceId, AuditInfo audit)
        {
            return new JoinInstanceEvent
            {
                InstanceId = instanceId,
            }.FillBase(audit, instanceId);
        }

        public static EditInstanceEvent EditEvent(Guid instanceId, string name, string joinCode, DateTime startDate, DateTime? endDate, AuditInfo audit)
        {
            return new EditInstanceEvent
            {
                Name = name,
                JoinCode = joinCode,
                StartDate = startDate,
                EndDate = endDate,
            }.FillBase(audit, instanceId);
        }
    }
}
