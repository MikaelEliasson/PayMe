using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Events
{
    public class AuditInfo
    {
        public DateTime TimeUtc { get; set; }
        public Guid UserId { get; set; }
        public string Ip { get; set; }
        public string UserAgent { get; set; }
    }
}
