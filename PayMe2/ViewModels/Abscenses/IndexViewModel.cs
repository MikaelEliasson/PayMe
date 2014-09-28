using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Abscenses
{
    public class IndexViewModel
    {
        public Guid InstanceId { get; set; }
        public List<AbscensesByPerson> Persons { get; set; }
    }

    public class AbscensesByPerson
    {
        public Guid UserId { get; set; }

        public List<servus.core.Entities.Abscense> Abscenses { get; set; }

        public int Sum { get; set; }

        public servus.core.Entities.ApplicationUser User { get; set; }
    }
}