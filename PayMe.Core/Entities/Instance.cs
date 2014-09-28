using PayMe.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace servus.core.Entities
{
    public class Instance
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JoinCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<UserToInstanceMapping> Users { get; set; }
        //public ICollection<Abscense> Abscenses { get; set; }
        //public ICollection<Expense> Expenses { get; set; }


        public DateTime CreatedAt { get; set; }
    }
}
