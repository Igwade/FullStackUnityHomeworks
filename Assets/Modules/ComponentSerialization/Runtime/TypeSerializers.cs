using System;
using System.Collections.Generic;

namespace Modules.ComponentSerialization.Runtime
{
    public static class TypeSerializers
    {
        private static readonly Dictionary<(Type, Type), object> Map = new();

        public static void Register<TSource, TDto>(ITypeSerializer<TSource, TDto> serializer)
        {
            Map[(typeof(TSource), typeof(TDto))] = serializer;
        }

        public static ITypeSerializer<TSource, TDto> Get<TSource, TDto>()
        {
            var key = (typeof(TSource), typeof(TDto));
            if (Map.TryGetValue(key, out var obj))
            {
                return (ITypeSerializer<TSource, TDto>)obj;
            }
            return null;
        }
    }
}