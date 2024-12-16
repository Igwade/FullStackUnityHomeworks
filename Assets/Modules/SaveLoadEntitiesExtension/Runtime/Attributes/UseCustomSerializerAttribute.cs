using System;

namespace SaveLoadEntitiesExtension.Attributes
{
    [AttributeUsage(System.AttributeTargets.Class)]
    public sealed class UseCustomSerializerAttribute : System.Attribute
    {
        public Type SerializerType { get; }
        public UseCustomSerializerAttribute(System.Type serializerType) { SerializerType = serializerType; }
    }
}