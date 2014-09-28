using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Instances
{
    public class JoinViewModel
    {
        [Required]
        public string JoinCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}