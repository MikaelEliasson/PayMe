using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Instances
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(8)]
        public string JoinCode { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
    }
}