using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Instances
{
    public class DetailsViewModel
    {
        public Instance Instance { get; set; }
        public List<EventForList> LastChanges { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public string GetName(Guid userId)
        {
            var user = Users.First(u => u.Id == userId);
            return user.FirstName + " " + user.LastName;
        }
    }
}