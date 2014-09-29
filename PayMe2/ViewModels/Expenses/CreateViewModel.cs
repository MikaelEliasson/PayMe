using Newtonsoft.Json;
using servus.core.Entities;
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

    public class CreateManyViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
        public Guid InstanceId { get; set; }
        public MvcHtmlString ToJson(object item)
        {
            return MvcHtmlString.Create(JsonConvert.SerializeObject(item));
        }
    }

    public class CreateManyDto
    {
        public decimal Sum { get; set; }
        public string Shop { get; set; }
        public DateTime Date { get; set; }
        public Guid CategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid[] AffectedUsers { get; set; }
    }
}