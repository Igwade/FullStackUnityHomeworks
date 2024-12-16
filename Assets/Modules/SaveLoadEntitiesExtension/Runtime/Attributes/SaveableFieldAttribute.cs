using System;

namespace SaveLoadEntitiesExtension.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SaveableAttribute : Attribute
    {
        public string OverrideName { get; }

        public SaveableAttribute(string overrideName = null)
        {
            OverrideName = overrideName;
        }
    }
}