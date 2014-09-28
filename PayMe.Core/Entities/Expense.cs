using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servus.core.Entities
{
    public class Expense
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public string Shop { get; set; }
        public ApplicationUser Owner { get; set; }
        public Guid OwnerId { get; set; }
        public Category Category { get; set; }
        public string AffectedUsers { get; set; }

        public Guid InstanceId { get; set; }
        public Instance Instance { get; set; }

        public DateTime Created { get; set; }

        public Guid CategoryId { get; set; }

    }
}
