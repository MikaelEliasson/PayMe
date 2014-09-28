using PayMe.Core.Events;
using servus.core.Entities;
using System.Linq;

namespace PayMe.Core.Eventprocessing.SqlModel
{
    public class CategoryEventHandler : EventHandlerBase
    {
        private Context db;
        public CategoryEventHandler(Context db)
        {
            this.db = db;
            Handles<CreateCategoryEvent>(Create);
            Handles<EditCategoryEvent>(Edit);
            Handles<DeleteCategoryEvent>(Delete);
        }

        public void Create(CreateCategoryEvent ev)
        {
            db.Categories.Add(new Category
            {
                Id = ev.CategoryId,
                Name = ev.Name,
                Type = ev.Type,
                DefaultUsers = ev.DefaultUsers != null ? string.Join(";", ev.DefaultUsers) : null,
                InstanceId = ev.InstanceId.Value,
            });
        }

        public void Edit(EditCategoryEvent ev)
        {
            var category = db.Categories.Find(ev.CategoryId);
            category.Name = ev.Name;
            category.Type = ev.Type;
            category.DefaultUsers = ev.DefaultUsers != null ? string.Join(";", ev.DefaultUsers) : null;
        }

        public void Delete(DeleteCategoryEvent ev)
        {
            var category = db.Categories.Find(ev.CategoryId);
            db.Categories.Remove(category);
        }
    }
}
