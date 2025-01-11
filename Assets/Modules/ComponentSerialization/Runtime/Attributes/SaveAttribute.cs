using System;

namespace Modules.ComponentSerialization.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class SaveAttribute : Attribute
    {
    }
}