#if UNITY_EDITOR

using UnityEngine;
using Modules.ComponentSerialization.Runtime;
using UnityEditor;

namespace Modules.ComponentSerialization
{
    public static partial class SaveCodeGenerator
    {
        /// <summary>
        /// Ищем все типы, которые реализуют ITypeSerializer<,>, чтобы уметь сериализовать сложные поля.
        /// </summary>
        private static void BuildSerializersMap()
        {
            SerializersMap.Clear();

            var candidates = TypeCache.GetTypesDerivedFrom(typeof(ITypeSerializer<,>));
            foreach (var st in candidates)
            {
                if (st.IsAbstract || st.IsInterface)
                    continue;

                var interfaces = st.GetInterfaces();
                foreach (var it in interfaces)
                {
                    if (!it.IsGenericType)
                        continue;
                    if (it.GetGenericTypeDefinition() != typeof(ITypeSerializer<,>))
                        continue;

                    var args = it.GetGenericArguments();
                    var source = args[0];
                    var dto = args[1];
                    if (!SerializersMap.ContainsKey(source))
                    {
                        SerializersMap[source] = (dto, st);
                    }
                    else
                    {
                        Debug.LogWarning($"[SaveCodeGenerator] Повторный сериализатор для {source.Name}: {st.FullName}");
                    }
                }
            }
        }
    }
}

#endif