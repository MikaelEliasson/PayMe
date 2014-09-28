using System;

namespace servus.core.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public string DefaultUsers { get; set; }

        public Guid InstanceId { get; set; }
        public Instance Instance { get; set; }
    }

    public enum CategoryType
    {
        SplitEqual = 0,
        BasedOnPresence = 1
    }
}
