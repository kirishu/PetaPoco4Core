using System;

namespace PetaPoco4Core.Test
{

    [AttributeUsage(AttributeTargets.Class)]
    public class RequiresCleanUpAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class DescriptionAttribute : Attribute
    {
        public string Description { get; set; }

        public DescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
