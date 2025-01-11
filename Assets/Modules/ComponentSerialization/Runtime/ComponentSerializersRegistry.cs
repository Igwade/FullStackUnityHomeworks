using System;
using UnityEngine;
using System.Collections.Generic;

namespace Modules.ComponentSerialization
{
    public static class ComponentSerializersRegistry
    {
        private static readonly Dictionary<Type, ComponentSerializer> Map = new();

        public static void Register(Type componentType, ComponentSerializer rec)
        {
            Map[componentType] = rec;
        }

        public static ComponentSerializer GetFor<T>(T component)
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