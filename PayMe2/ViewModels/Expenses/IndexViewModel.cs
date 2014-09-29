using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayMe2.ViewModels.Expenses
{
    public class IndexViewModel
    {
        public Guid InstanceId { get; set; }

        public IDictionary<Guid, servus.core.Entities.ApplicationUser> Persons { get; set; }

        public IEnumerable<servus.core.Entities.Expense> Expenses { get; set; }

        public Dictionary<Guid, servus.core.Entities.Category> Categories { get; set; }

        public MvcHtmlString GetUsers(servus.core.Entities.Expense item)
        {
            var users = item.AffectedUsers.Split(';');
            return MvcHtmlString.Create(string.Join(", ", users.Select(str => Persons[Guid.Parse(str)].GetName())));
        }
    }
}