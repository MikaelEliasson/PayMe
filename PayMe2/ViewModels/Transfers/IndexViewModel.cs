using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Transfers
{
    public class IndexViewModel
    {
        public List<PayMe.Core.Entities.Transfer> Transfers { get; set; }
        public Guid InstanceId { get; set; }
        public Dictionary<Guid, servus.core.Entities.ApplicationUser> Persons { get; set; }
    }
}