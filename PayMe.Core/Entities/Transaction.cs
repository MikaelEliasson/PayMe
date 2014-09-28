using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servus.core.Entities
{
    public class Transaction
    {
        public string Id { get; set; }
        public ApplicationUser Sender { get; set; }
        public Guid SenderId { get; set; }
        public ApplicationUser Reciever { get; set; }
        public int ReciverId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool Confirmed { get; set; }
    }
}
