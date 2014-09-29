using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PayMe2.ViewModels.Expenses
{
    public class CreateViewModel
    {
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public string Shop { get; set; }
        public Guid Category { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        public Guid[] AffectedUsers { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

        public Guid InstanceId { get; set; }
    }
}