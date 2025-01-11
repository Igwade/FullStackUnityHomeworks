#if UNITY_EDITOR

using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Modules.ComponentSerialization.Runtime.Attributes;

namespace Modules.ComponentSerialization
{
    public static partial class SerializationCodeGenerator
    {
        private static string GenerateDtoAndSerializerForComponent(
            Type compType,
            out string dtoName,
            out string serializerName
        )
        {
            dtoName = compType.Name + "Dto";
            serializerName = compType.Name + "Serializer";

            var sb = new StringBuilder();
            sb.AppendLine($"    // ----- {compType.Name} -----");
            sb.AppendLine();

            var members = GetSaveMembers(compType);

            sb.AppendLine($"    [Serializable]");
            sb.AppendLine($"    public class {dtoName}");
            sb.AppendLine("    {");
            foreach (var m in members)
            {
                var dtoFieldType = ResolveDtoFieldType(m);
                sb.AppendLine($"        public {dtoFieldType} {m.Name};");
            }

            sb.AppendLine("    }");
            sb.AppendLine();

            sb.AppendLine($"    public static class {serializerName}");
            sb.AppendLine("    {");
            sb.AppendLine($"        public static {dtoName} Serialize({compType.FullName} source)");
            sb.AppendLine("        {");
            sb.AppendLine($"            var dto = new {dtoName}();");
            foreach (var m in members)
            {
                sb.AppendLine(GenerateSerializeLine("source", "dto", m));
            }

            sb.AppendLine("            return dto;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine($"        public static void Deserialize({compType.FullName} target, {dtoName} dto)");
            sb.AppendLine("        {");
            foreach (var m in members)
            {
                sb.AppendLine(GenerateDeserializeLine("target", "dto", m));
            }

            sb.AppendLine("        }");
            sb.AppendLine();
           /*
            ComponentSerializersRegistry.Register(typeof(Game.Scripts.Gameplay.Components.Health), new ComponentSerializer
            {
                DtoType = typeof(Modules.ComponentSerialization.HealthDto),
                Serialize = (mono) => Modules.ComponentSerialization.HealthSerializer.Serialize((Game.Scripts.Gameplay.Components.Health)mono),
                Deserialize = (mono, dto) => Modules.ComponentSerialization.HealthSerializer.Deserialize((Game.Scripts.Gameplay.Components.Health)mono, (Modules.ComponentSerialization.HealthDto)dto)
            });
            */
            sb.AppendLine($"        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]");
            sb.AppendLine($"        private static void Register()");
            sb.AppendLine($"        {{");
            sb.AppendLine($"            {nameof(ComponentSerializersRegistry)}.Register(typeof({compType.FullName}), new {nameof(ComponentSerializer)}");
            sb.AppendLine($"            {{");
            sb.AppendLine($"                DtoType = typeof({dtoName}),");
            sb.AppendLine($"                Serialize = (mono) => Serialize(({compType.FullName})mono),");
            sb.AppendLine($"                Deserialize = (mono, dto) => Deserialize(({compType.FullName})mono, ({dtoName})dto)");
            sb.AppendLine($"            }});");
            sb.AppendLine($"        }}");
            sb.AppendLine($"    }}");

            return sb.ToString();
        }

        private static List<MemberInfo> GetSaveMembers(Type compType)
        {
            var list = new List<MemberInfo>();

            var fields = compType.GetFields(BindingFlags.Instance | BindingFlags.Public)
                .Where(f => Attribute.IsDefined(f, typeof(SaveAttribute)));
            list.AddRange(fields);

            var props = compType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => Attribute.IsDefined(p, typeof(SaveAttribute)));
            list.AddRange(props);

            return list;
        }

        /// <summary>
        /// Определяем, как будет выглядеть тип поля в DTO (строка кода).
        /// Если это enum — string, массив enum — string[], коллекции enum — List<string>, 
        /// иначе используем сериализатор из SerializersMap.
        /// </summary>
        private static string ResolveDtoFieldType(MemberInfo member)
        {
            var t = GetMemberType(member);

            if (IsEnum(t))
            {
                return "string";
            }

            if (t.IsArray)
            {
                var elemType = t.GetElementType();

                if (IsEnum(elemType))
                {
                    return "string[]";
                }

                if (SerializersMap.TryGetValue(elemType, out var pair))
                {
                    return $"{pair.dtoType.FullName}[]";
                }

                if (IsPrimitiveOrString(elemType))
                {
                    return $"{elemType.Name}[]";
                }

                return $"{elemType.Name}[]";
            }

            if (IsCollectionOfT(t, out var collectionElemType))
            {
                if (IsEnum(collectionElemType))
                {
                    return "List<string>";
                }

                if (SerializersMap.TryGetValue(collectionElemType, out var pair2))
                {
                    return $"List<{pair2.dtoType.FullName}>";
                }

                if (IsPrimitiveOrString(collectionElemType))
                {
                    return $"List<{collectionElemType.FullName}>";
                }

                return $"List<{collectionElemType.FullName}>";
            }

            if (SerializersMap.TryGetValue(t, out var pair3))
            {
                return pair3.dtoType.FullName;
            }

            if (IsPrimitiveOrString(t))
            {
                return t.Name;
            }

            return t.Name;
        }

        private static string GenerateSerializeLine(string sourceVar, string dtoVar, MemberInfo m)
        {
            var t = GetMemberType(m);
            var name = m.Name;

            if (t.IsArray)
            {
                var elemType = t.GetElementType();
                return GenerateArraySerialize(sourceVar, dtoVar, name, elemType);
            }

            if (IsCollectionOfT(t, out var listElemType))
            {
                return GenerateCollectionSerialize(sourceVar, dtoVar, name, listElemType);
            }

            return GenerateSingleSerialize(sourceVar, dtoVar, name, t);
        }

        private static string GenerateDeserializeLine(string targetVar, string dtoVar, MemberInfo m)
        {
            var t = GetMemberType(m);
            var name = m.Name;

            if (t.IsArray)
            {
                var elemType = t.GetElementType();
                return GenerateArrayDeserialize(targetVar, dtoVar, name, elemType);
            }

            if (IsCollectionOfT(t, out var listElemType))
            {
                return GenerateCollectionDeserialize(targetVar, dtoVar, name, listElemType);
            }

            return GenerateSingleDeserialize(targetVar, dtoVar, name, t);
        }

        private static string GenerateArraySerialize(string sourceVar, string dtoVar, string memberName, Type elemType)
        {
            if (IsEnum(elemType))
            {
                return $@"            if ({sourceVar}.{memberName} != null)
            {{
                {dtoVar}.{memberName} = new string[{sourceVar}.{memberName}.Length];
                for (int i = 0; i < {sourceVar}.{memberName}.Length; i++)
                {{
                    {dtoVar}.{memberName}[i] = {sourceVar}.{memberName}[i].ToString();
                }}
            }}
            else
            {{
                {dtoVar}.{memberName} = null;
            }}";
            }

            if (SerializersMap.TryGetValue(elemType, out var pair))
            {
                return
                    $@"            if ({sourceVar}.{memberName} != null)
            {{
                {dtoVar}.{memberName} = new {pair.dtoType.FullName}[{sourceVar}.{memberName}.Length];
                for (int i=0; i < {sourceVar}.{memberName}.Length; i++)
                {{
                    var serializer = {TypeSerializersName}.Get<{elemType.FullName}, {pair.dtoType.FullName}>();
                    {dtoVar}.{memberName}[i] = serializer.Serialize({sourceVar}.{memberName}[i]);
                }}
            }}
            else
            {{
                {dtoVar}.{memberName} = null;
            }}";
            }
            else if (IsPrimitiveOrString(elemType))
            {
                return
                    $@"            {dtoVar}.{memberName} = {sourceVar}.{memberName}?.Clone() as {elemType.Name}[];";
            }

            return
                $@"            {dtoVar}.{memberName} = {sourceVar}.{memberName};";
        }

        private static string GenerateCollectionSerialize(string sourceVar, string dtoVar, string memberName,
            Type elemType)
        {
            if (IsEnum(elemType))
            {
                return
                    $@"            if ({sourceVar}.{memberName} != null)
            {{
                var sourceCollection = {sourceVar}.{memberName};
                {dtoVar}.{memberName} = new System.Collections.Generic.List<string>();
                foreach (var item in sourceCollection)
                {{
                    {dtoVar}.{memberName}.Add(item.ToString());
                }}
            }}
            else
            {{
                {dtoVar}.{memberName} = null;
            }}";
            }

            if (SerializersMap.TryGetValue(elemType, out var pair))
            {
                return
                    $@"            if ({sourceVar}.{memberName} != null)
            {{
                var sourceCollection = {sourceVar}.{memberName};
                {dtoVar}.{memberName} = new System.Collections.Generic.List<{pair.dtoType.FullName}>();
                var serializer = {TypeSerializersName}.Get<{elemType.FullName}, {pair.dtoType.FullName}>();

                foreach (var item in sourceCollection)
                {{
                    {dtoVar}.{memberName}.Add(serializer.Serialize(item));
                }}
            }}
            else
            {{
                {dtoVar}.{memberName} = null;
            }}";
            }
            else if (IsPrimitiveOrString(elemType))
            {
                return
                    $@"            if ({sourceVar}.{memberName} != null)
            {{
                var sourceCollection = {sourceVar}.{memberName};
                {dtoVar}.{memberName} = new System.Collections.Generic.List<{elemType.FullName}>(System.Linq.Enumerable.Cast<{elemType.Name}>(sourceCollection));
            }}
            else
            {{
                {dtoVar}.{memberName} = null;
            }}";
            }

            return
                $@"            {dtoVar}.{memberName} = {sourceVar}.{memberName};";
        }

        private static string GenerateSingleSerialize(string sourceVar, string dtoVar, string memberName, Type t)
        {
            if (IsEnum(t))
            {
                return $@"            {dtoVar}.{memberName} = {sourceVar}.{memberName}.ToString();";
            }

            if (SerializersMap.TryGetValue(t, out var pair))
            {
                return
                    $@"            {{
                var serializer = {TypeSerializersName}.Get<{t.FullName}, {pair.dtoType.FullName}>();
                {dtoVar}.{memberName} = serializer.Serialize({sourceVar}.{memberName});
            }}";
            }
            else if (IsPrimitiveOrString(t))
            {
                return $@"            {dtoVar}.{memberName} = {sourceVar}.{memberName};";
            }

            return $@"            {dtoVar}.{memberName} = {sourceVar}.{memberName};";
        }

        private static string GenerateArrayDeserialize(string targetVar, string dtoVar, string memberName,
            Type elemType)
        {
            if (IsEnum(elemType))
            {
                return
                    $@"            if ({dtoVar}.{memberName} != null)
            {{
                {targetVar}.{memberName} = new {elemType.FullName}[{dtoVar}.{memberName}.Length];
                for (int i = 0; i < {dtoVar}.{memberName}.Length; i++)
                {{
                    {targetVar}.{memberName}[i] = ({elemType.FullName})System.Enum.Parse(typeof({elemType.FullName}), {dtoVar}.{memberName}[i]);
                }}
            }}
            else
            {{
                {targetVar}.{memberName} = null;
            }}";
            }

            if (SerializersMap.TryGetValue(elemType, out var pair))
            {
                return
                    $@"            if ({dtoVar}.{memberName} != null)
            {{
                {targetVar}.{memberName} = new {elemType.Name}[{dtoVar}.{memberName}.Length];
                var serializer = {TypeSerializersName}.Get<{elemType.FullName}, {pair.dtoType.FullName}>();
                for (int i=0; i < {dtoVar}.{memberName}.Length; i++)
                {{
                    {targetVar}.{memberName}[i] = serializer.Deserialize({dtoVar}.{memberName}[i]);
                }}
            }}
            else
            {{
                {targetVar}.{memberName} = null;
            }}";
            }
            else if (IsPrimitiveOrString(elemType))
            {
                return
                    $@"            if ({dtoVar}.{memberName} != null)
            {{
                {targetVar}.{memberName} = ({elemType.Name}[]){dtoVar}.{memberName}.Clone();
            }}
            else
            {{
                {targetVar}.{memberName} = null;
            }}";
            }

            return
                $@"            {targetVar}.{memberName} = {dtoVar}.{memberName};";
        }

        private static string GenerateCollectionDeserialize(string targetVar, string dtoVar, string memberName,
            Type elemType)
        {
            if (IsEnum(elemType))
            {
                return
                    $@"            if ({dtoVar}.{memberName} != null)
            {{
                var newCollection = new System.Collections.Generic.List<{elemType.FullName}>({dtoVar}.{memberName}.Count);
                foreach (var dtoItem in {dtoVar}.{memberName})
                {{
                    newCollection.Add(({elemType.FullName})System.Enum.Parse(typeof({elemType.FullName}), dtoItem));
                }}
                {targetVar}.{memberName} = newCollection;
            }}
            else
            {{
                {targetVar}.{memberName} = null;
            }}";
            }

            if (SerializersMap.TryGetValue(elemType, out var pair))
            {
                return
                    $@"            if ({dtoVar}.{memberName} != null)
            {{
                var serializer = {TypeSerializersName}.Get<{elemType.FullName}, {pair.dtoType.FullName}>();
                var newCollection = new System.Collections.Generic.List<{elemType.FullName}>({dtoVar}.{memberName}.Count);
                foreach (var dtoItem in {dtoVar}.{memberName})
                {{
                    newCollection.Add(serializer.Deserialize(dtoItem));
                }}
                {targetVar}.{memberName} = newCollection;
            }}
            else
            {{
                {targetVar}.{memberName} = null;
            }}";
            }

            if (IsPrimitiveOrString(elemType))
            {
                return
                    $@"            if ({dtoVar}.{memberName} != null)
            {{
                {targetVar}.{memberName} = new System.Collections.Generic.List<{elemType.FullName}>({dtoVar}.{memberName});
            }}
            else
            {{
                {targetVar}.{memberName} = null;
            }}";
            }

            return
                $@"            {targetVar}.{memberName} = {dtoVar}.{memberName};";
        }

        private static string GenerateSingleDeserialize(string targetVar, string dtoVar, string memberName, Type t)
        {
            if (IsEnum(t))
            {
                return
                    $@"            {targetVar}.{memberName} = ({t.FullName})System.Enum.Parse(typeof({t.FullName}), {dtoVar}.{memberName});";
            }

            if (SerializersMap.TryGetValue(t, out var pair))
            {
                return
                    $@"            {{
                var serializer = {TypeSerializersName}.Get<{t.FullName}, {pair.dtoType.FullName}>();
                {targetVar}.{memberName} = serializer.Deserialize({dtoVar}.{memberName});
            }}";
            }
            else if (IsPrimitiveOrString(t))
            {
                return $@"            {targetVar}.{memberName} = {dtoVar}.{memberName};";
            }

            return $@"            {targetVar}.{memberName} = {dtoVar}.{memberName};";
        }
    }
}

#endif