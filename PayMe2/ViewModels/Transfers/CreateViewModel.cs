using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Transfers
{
    public class CreateViewModel
    {
        public Guid InstanceId { get; set; }

        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
        public Guid ToUserId { get; set; }

        public List<System.Web.Mvc.SelectListItem> Users { get; set; }
    }
}