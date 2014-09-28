using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PayMe2.ViewModels.Categories
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }

        public CategoryType Type { get; set; }

        public Guid[] DefaultUsers { get; set; }

        public IEnumerable<SelectListItem> TypeList { get{
            yield return new SelectListItem { Text = "Split equal", Value = CategoryType.SplitEqual.ToString() };
            yield return new SelectListItem { Text = "Based on presence", Value = CategoryType.BasedOnPresence.ToString() };
        }}

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}