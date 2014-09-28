using PayMe.Core.Entities;
using PayMe.Core.Events;
using System.Collections.Generic;

namespace PayMe.Core.Eventprocessing.SqlModel
{
    public class InstanceEventHandler : EventHandlerBase
    {
        private Context db;
        public InstanceEventHandler(Context db)
        {
            this.db = db;
            Handles<CreateInstanceEvent>(Create);
            Handles<EditInstanceEvent>(Edit);
            Handles<JoinInstanceEvent>(Join);
        }

        public void Create(CreateInstanceEvent ev)
        {
            db.Instances.Add(new servus.core.Entities.Instance
            {
                Name = ev.Name,
                Id = ev.InstanceId.Value,
                JoinCode = ev.JoinCode,
                StartDate = ev.StartDate,
                EndDate = ev.EndDate,
                CreatedAt = ev.TimeUtc,
                Users = new List<UserToInstanceMapping>{
                    new UserToInstanceMapping{
                        UserId = ev.UserId,
                        JoinDateUtc = ev.TimeUtc,
                        Creator = true
                    }
                }
            });
        }

        public void Edit(EditInstanceEvent ev)
        {
            var instance = db.Instances.Find(ev.InstanceId);
            instance.Name = ev.Name;
            instance.JoinCode = ev.JoinCode;
            instance.StartDate = ev.StartDate;
            instance.EndDate = ev.EndDate;
        }

        public void Join(JoinInstanceEvent ev)
        {
            db.UserToInstanceMappings.Add(new UserToInstanceMapping
            {
                InstanceId = ev.InstanceId.Value,
                UserId = ev.UserId,
                JoinDateUtc = ev.TimeUtc
            });
        }
    }
}
