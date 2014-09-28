using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Instances
{
    public class ListViewModel
    {
        public IEnumerable<Instance> Instances { get; set; }
    }
}