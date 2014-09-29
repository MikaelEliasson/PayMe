using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servus.core.Entities
{
    public class Abscense
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid InstanceId { get; set; }

        public ApplicationUser User { get; set; }
        public Instance Instance { get; set; }

        public string Description { get; set; }

    }
}
