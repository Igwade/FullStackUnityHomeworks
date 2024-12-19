using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using SaveLoad;
using SaveLoadEntitiesExtension;
using SaveLoadEntitiesExtension.Attributes;
using UnityEditor;
using UnityEngine;

namespace SaveLoadEntitiesExtension
{
    public static class SaveLoadCodeGenerator
    {
        [MenuItem("SaveLoad/Generate Entities Serialization Code")]
        public static void GenerateCodeManual()
        {
            var config = LoadConfig();
            if (config == null)
            {
                Debug.LogError("No SaveLoadGenerationConfig found! Create one via Assets/Create/SaveLoad/GenerationConfig");
                return;
            }

            GenerateCode(config);
            Debug.Log("Save/Load code generation completed.");
        }

        [InitializeOnLoadMethod]
        private static void OnEditorLoad()
        {
            EditorApplication.delayCall += () =>
            {
                var config = LoadConfig();
                if (config != null && config.RunOnCompile)
                {
                    GenerateCode(config);
                }
            };
        }

        private static SaveLoadGenerationConfig LoadConfig()
        {
            var guids = AssetDatabase.FindAssets("t:SaveLoadGenerationConfig");
            if (guids.Length == 0) return null;
            return AssetDatabase.LoadAssetAtPath<SaveLoadGenerationConfig>(AssetDatabase.GUIDToAssetPath(guids[0]));
        }

        public static void GenerateCode(SaveLoadGenerationConfig config)
        {
            CleanUpExistingFiles(config);
            Directory.CreateDirectory(config.GeneratedCodeOutputPath);

            var assembliesToScan = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => config.AssembliesToScan.Contains(a.GetName().Name))
                .ToArray();

            foreach (var assembly in assembliesToScan)
            {
                GenerateCodeForAssembly(assembly, config);
            }

            AssetDatabase.Refresh();
        }

        private static void CleanUpExistingFiles(SaveLoadGenerationConfig config)
        {
            if (!Directory.Exists(config.GeneratedCodeOutputPath)) return;

            var existingGeneratedFiles = Directory.GetFiles(config.GeneratedCodeOutputPath, "*" + config.FileSuffix + ".cs");
            foreach (var file in existingGeneratedFiles)
            {
                File.Delete(file);
            }
        }

        private static void GenerateCodeForAssembly(Assembly assembly, SaveLoadGenerationConfig config)
        {
            foreach (var type in assembly.GetTypes())
            {
                var saveComponentAttr = type.GetCustomAttribute<SaveComponentAttribute>();
                if (saveComponentAttr == null) continue;

                ValidateSaveableMembers(type);

                var saveableFields = GetSaveableFields(type);
                var saveableProperties = GetSaveableProperties(type);
                var customSerializer = type.GetCustomAttribute<UseCustomSerializerAttribute>();

                var generatedCode = GenerateTypeCode(
                    config.GeneratedNamespace,
                    type,
                    saveableFields,
                    saveableProperties,
                    customSerializer,
                    config.AdditionalNamespaces
                );

                WriteGeneratedCodeToFile(config, type, generatedCode);
            }
        }

        private static void WriteGeneratedCodeToFile(SaveLoadGenerationConfig config, Type targetType, string generatedCode)
        {
            string fileName = targetType.Name + config.FileSuffix + ".cs";
            string filePath = Path.Combine(config.GeneratedCodeOutputPath, fileName);
            File.WriteAllText(filePath, generatedCode);
        }

        private static void ValidateSaveableMembers(Type type)
        {
            ValidateSaveableFields(type);
            ValidateSaveableProperties(type);
        }

        private static void ValidateSaveableFields(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                             .Where(f => f.GetCustomAttribute<SaveableAttribute>() != null);

            foreach (var field in fields)
            {
                bool isPublicField = field.IsPublic;
                bool isPublicBacking = IsAutoPropertyBackingFieldPublic(type, field);

                if (!isPublicField && !isPublicBacking)
                {
                    throw new ArgumentException(
                        $"Field '{GetFieldName(field)}' in '{type.FullName}' marked with [Saveable] must have a public get and set."
                    );
                }
            }
        }

        private static bool IsAutoPropertyBackingFieldPublic(Type type, FieldInfo field)
        {
            if (!field.Name.Contains("__BackingField")) return false;

            string propertyName = GetFieldName(field);
            var getter = type.GetMethod($"get_{propertyName}");
            var setter = type.GetMethod($"set_{propertyName}");

            return getter != null && setter != null && getter.IsPublic && setter.IsPublic;
        }

        private static void ValidateSaveableProperties(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => p.GetCustomAttribute<SaveableAttribute>() != null);

            foreach (var property in properties)
            {
                bool canReadPublic = property.CanRead && property.GetMethod?.IsPublic == true;
                bool canWritePublic = property.CanWrite && property.SetMethod?.IsPublic == true;

                if (!canReadPublic || !canWritePublic)
                {
                    throw new ArgumentException(
                        $"Property '{GetPropertyName(property)}' in '{type.FullName}' marked with [Saveable] must have both a public getter and setter."
                    );
                }
            }
        }

        private static FieldInfo[] GetSaveableFields(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                       .Where(f => f.GetCustomAttribute<SaveableAttribute>() != null)
                       .ToArray();
        }

        private static PropertyInfo[] GetSaveableProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                       .Where(p => p.GetCustomAttribute<SaveableAttribute>() != null)
                       .ToArray();
        }

        private static string GetPropertyName(PropertyInfo prop)
        {
            var saveableAttribute = prop.GetCustomAttribute<SaveableAttribute>();
            var overrideName = saveableAttribute.OverrideName;
            var propertyName = overrideName ?? prop.Name;

            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));

            return propertyName;
        }
        
        private static string GetFieldName(FieldInfo field)
        {
            var saveableAttribute = field.GetCustomAttribute<SaveableAttribute>();
            var overrideName = saveableAttribute.OverrideName;
            var fieldName = overrideName ?? field.Name;

            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException("Field name cannot be null or empty.", nameof(fieldName));

            // Handle auto-property backing fields.
            var match = Regex.Match(fieldName, @"^<(.+?)>k__BackingField$");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return fieldName;
        }

        private static string GenerateTypeCode(
            string namespaceName,
            Type targetType,
            FieldInfo[] saveableFields,
            PropertyInfo[] saveableProperties,
            UseCustomSerializerAttribute customSerializer,
            string[] additionalNamespaces
        )
        {
            var sb = new StringBuilder();

            sb.AppendLine("using SaveLoadEntitiesExtension;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using System.Collections.Generic;");
            foreach (var ns in additionalNamespaces)
            {
                sb.AppendLine($"using {ns};");
            }
            sb.AppendLine();

            if (!string.IsNullOrEmpty(namespaceName))
            {
                sb.AppendLine($"namespace {namespaceName}");
                sb.AppendLine("{");
            }

            string typeName = targetType.Name;

            sb.AppendLine($"    public static class {typeName}_Serialization");
            sb.AppendLine("    {");

            if (customSerializer == null)
            {
                GenerateDataClass(sb, typeName, saveableFields, saveableProperties);
            }

            GenerateSaveMethod(sb, typeName, targetType, saveableFields, saveableProperties, customSerializer);
            GenerateLoadMethod(sb, typeName, targetType, saveableFields, saveableProperties, customSerializer);
            GenerateRegisterMethod(sb, typeName);

            sb.AppendLine("    }"); // class end

            if (!string.IsNullOrEmpty(namespaceName))
            {
                sb.AppendLine("}"); // namespace end
            }

            return sb.ToString();
        }
        
        private static void GenerateDataClass(
            StringBuilder sb,
            string typeName,
            IEnumerable<FieldInfo> fields,
            IEnumerable<PropertyInfo> properties
        )
        {
            sb.AppendLine($"        private class {typeName}Data");
            sb.AppendLine("        {");

            foreach (var field in fields)
            {
                sb.AppendLine($"            public string {GetFieldName(field)};");
            }
            foreach (var prop in properties)
            {
                sb.AppendLine($"            public string {GetPropertyName(prop)};");
            }

            sb.AppendLine("        }");
            sb.AppendLine();
        }
        
        private static void GenerateSaveMethod(
            StringBuilder sb,
            string typeName,
            Type targetType,
            FieldInfo[] saveableFields,
            PropertyInfo[] saveableProperties,
            UseCustomSerializerAttribute customSerializer
        )
        {
            sb.AppendLine($"        public static string Save_{typeName}(IComponent c, ISerializer serializer, ISaveLoadContext context)");
            sb.AppendLine("        {");

            if (customSerializer != null)
            {
                string customSerializerTypeName = GetFriendlyTypeName(customSerializer.SerializerType);
                sb.AppendLine($"            return {customSerializerTypeName}.Serialize(c, serializer, context);");
            }
            else
            {
                sb.AppendLine($"            var comp = c.GetRawComponent() as {targetType.FullName};");
                sb.AppendLine($"            var data = new {typeName}Data();");
                sb.AppendLine();

                foreach (var field in saveableFields)
                {
                    string fieldName = GetFieldName(field);
                    sb.AppendLine($"            data.{fieldName} = serializer.Serialize(comp.{fieldName});");
                }

                foreach (var prop in saveableProperties)
                {
                    string propName = GetPropertyName(prop);
                    sb.AppendLine($"            data.{propName} = serializer.Serialize(comp.{propName});");
                }

                sb.AppendLine();
                sb.AppendLine("            return serializer.Serialize(data);");
            }

            sb.AppendLine("        }");
            sb.AppendLine();
        }
        
        private static void GenerateLoadMethod(
            StringBuilder sb,
            string typeName,
            Type targetType,
            FieldInfo[] saveableFields,
            PropertyInfo[] saveableProperties,
            UseCustomSerializerAttribute customSerializer
        )
        {
            sb.AppendLine($"        public static void Load_{typeName}(IComponent c, string json, ISerializer serializer, ISaveLoadContext context)");
            sb.AppendLine("        {");

            if (customSerializer != null)
            {
                string customSerializerTypeName = GetFriendlyTypeName(customSerializer.SerializerType);
                sb.AppendLine($"            {customSerializerTypeName}.Deserialize(c, json, serializer, context);");
            }
            else
            {
                sb.AppendLine($"            var comp = c.GetRawComponent() as {targetType.FullName};");
                sb.AppendLine($"            var data = serializer.Deserialize<{typeName}Data>(json);");
                sb.AppendLine();

                foreach (var field in saveableFields)
                {
                    string fieldName = GetFieldName(field);
                    sb.AppendLine($"            comp.{fieldName} = serializer.Deserialize<{GetFriendlyTypeName(field.FieldType)}>(data.{fieldName});");
                }

                foreach (var prop in saveableProperties)
                {
                    string propName = GetPropertyName(prop);
                    sb.AppendLine($"            comp.{propName} = serializer.Deserialize<{GetFriendlyTypeName(prop.PropertyType)}>(data.{propName});");
                }
            }

            sb.AppendLine("        }");
            sb.AppendLine();
        }
        
        private static void GenerateRegisterMethod(StringBuilder sb, string typeName)
        {
            sb.AppendLine("        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]");
            sb.AppendLine("        public static void Register()");
            sb.AppendLine("        {");
            sb.AppendLine($"            SaveableComponentRegistry.Register(\"{typeName}\", Save_{typeName}, Load_{typeName});");
            sb.AppendLine("        }");
        }

        private static string GetFriendlyTypeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.FullName!.Replace("+", ".");
            }

            var genericTypeName = type.GetGenericTypeDefinition().FullName!.Replace("+", ".");
            var genericArgs = type.GetGenericArguments().Select(GetFriendlyTypeName);
            var genericArgsString = string.Join(", ", genericArgs);

            var baseName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
            return $"{baseName}<{genericArgsString}>";
        }
    }
}
