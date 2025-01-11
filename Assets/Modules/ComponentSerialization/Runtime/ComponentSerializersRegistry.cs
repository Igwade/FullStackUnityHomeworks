// Auto-generated by SaveCodeGenerator
namespace Modules.ComponentSerialization
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    public static class ComponentSerializersRegistry
    {
        private static readonly Dictionary<Type, ComponentSerializer> Map = new();
        
        public static void Register(Type componentType, ComponentSerializer rec)
        {
            Map[componentType] = rec;
        }

        public static ComponentSerializer GetRecord(Type t)
        {
            Map.TryGetValue(t, out var rec);
            return rec;
        }

        public static ComponentSerializer GetRecord<T>(T component) where T : MonoBehaviour
        {
            Map.TryGetValue(component.GetType(), out var rec);
            return rec;
        }
    }

    public class ComponentSerializer
    {
        public Type DtoType;
        public Func<MonoBehaviour, object> Serialize;
        public Action<MonoBehaviour, object> Deserialize;
    }
}
