using servus.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMe.Core.Events
{
    public class CreateCategoryEvent : EventBase
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public Guid[] DefaultUsers { get; set; }
        public CategoryType Type { get; set; }
    }

    public class EditCategoryEvent : CreateCategoryEvent
    {

    }

    public class DeleteCategoryEvent : EventBase
    {
        public Guid CategoryId { get; set; }
    }

    public static class CategoryEventFactory
    {
        public static CreateCategoryEvent CreateCategory(Guid instanceId, Guid categoryId, string name, Guid[] defaultUsers, CategoryType type, AuditInfo audit)
        {
            return new CreateCategoryEvent
            {
                CategoryId = categoryId,
                Name = name,
                DefaultUsers = defaultUsers,
                Type = type
            }.FillBase(audit, instanceId);
        }

        public static EditCategoryEvent EditCategory(Guid instanceId, Guid categoryId, string name, Guid[] defaultUsers, CategoryType type, AuditInfo audit)
        {
            return new EditCategoryEvent
            {
                CategoryId = categoryId,
                Name = name,
                DefaultUsers = defaultUsers,
                Type = type
            }.FillBase(audit, instanceId);
        }

        public static DeleteCategoryEvent DeleteCategory(Guid instanceId, Guid categoryId, AuditInfo audit)
        {
            return new DeleteCategoryEvent
            {
                CategoryId = categoryId,
            }.FillBase(audit, instanceId);
        }
    }
}
