using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Abscenses
{
    public class CreateViewModel
    {
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        public Guid InstanceId { get; set; }
    }
}