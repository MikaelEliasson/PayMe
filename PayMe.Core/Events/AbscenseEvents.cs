using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Events
{
    public class CreateAbscenseEvent : EventBase
    {
        public Guid AbscenseId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class EditAbscenseEvent : CreateAbscenseEvent
    {
    }

    public class DeleteAbscenseEvent : EventBase
    {
        public Guid AbscenseId { get; set; }
    }

    public static class AbscenseEventFactory
    {
        public static CreateAbscenseEvent CreateAbscense(Guid instanceId, Guid abscenseId, string description, DateTime startDate, DateTime endDate, AuditInfo audit)
        {
            return new CreateAbscenseEvent
            {
                AbscenseId = abscenseId,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
            }.FillBase(audit, instanceId);
        }

        public static EditAbscenseEvent EditAbscense(Guid instanceId, Guid abscenseId, string description, DateTime startDate, DateTime endDate, AuditInfo audit)
        {
            return new EditAbscenseEvent
            {
                AbscenseId = abscenseId,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
            }.FillBase(audit, instanceId);
        }

        public static DeleteAbscenseEvent DeleteAbscense(Guid instanceId, Guid abscenseId, AuditInfo audit)
        {
            return new DeleteAbscenseEvent
            {
                AbscenseId = abscenseId,
            }.FillBase(audit, instanceId);
        }
    }
}
