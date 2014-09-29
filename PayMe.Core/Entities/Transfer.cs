using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Entities
{
    public class Transfer
    {
        public Guid Id { get; set; }
        public ApplicationUser FromUser { get; set; }
        public Guid FromUserId { get; set; }
        public ApplicationUser ToUser { get; set; }
        public Guid ToUserId { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }

        public Guid InstanceId { get; set; }
        public Instance Instance { get; set; }
    }
}
