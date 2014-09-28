using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PayMe2.ViewModels.Categories
{
    public class IndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public Guid InstanceId { get; set; }

        public string GetUsersText(string users)
        {
            if (string.IsNullOrWhiteSpace(users))
            {
                return "All users";
            }

            var parts = users.Split(';').Select(str =>
            {
                var user = Users[Guid.Parse(str)];
                return user.GetName();
            });

            return string.Join(", ", parts);
        }

        public Dictionary<Guid, ApplicationUser> Users { get; set; }
    }
}