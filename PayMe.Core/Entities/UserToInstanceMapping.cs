using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Entities
{
    public class UserToInstanceMapping
    {
        public Guid UserId { get; set; }
        public Guid InstanceId { get; set; }
        public DateTime JoinDateUtc { get; set; }
        public bool Creator { get; set; }

        public ApplicationUser User { get; set; }
        public Instance Instance { get; set; }

    }
}
