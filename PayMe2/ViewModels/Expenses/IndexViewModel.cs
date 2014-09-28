using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Expenses
{
    public class IndexViewModel
    {
        public Guid InstanceId { get; set; }

        public IDictionary<Guid, servus.core.Entities.ApplicationUser> Persons { get; set; }

        public IEnumerable<servus.core.Entities.Expense> Expenses { get; set; }
    }
}