using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SaveLoadEntitiesExtension;
using SaveLoadEntitiesExtension.Attributes;
using UnityEditor;
using UnityEngine;

namespace SaveLoadEntitiesExtension
{
    public static class SaveLoadCodeGenerator
    {
        [MenuItem("SaveLoad/Generate Serialization Code")]
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
        static void OnEditorLoad()
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

        private static void GenerateCode(SaveLoadGenerationConfig config)
        {
            if (Directory.Exists(config.GeneratedCodeOutputPath))
            {
                foreach (var file in Directory.GetFiles(config.GeneratedCodeOutputPath, "*" + config.FileSuffix + ".cs"))
                {
                    File.Delete(file);
                }
            }
            
            Directory.CreateDirectory(config.GeneratedCodeOutputPath);

            foreach (var file in Directory.GetFiles(config.GeneratedCodeOutputPath, "*" + config.FileSuffix + ".cs"))
            {
                File.Delete(file);
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => config.AssembliesToScan.Contains(a.GetName().Name))
                .ToArray();

            foreach (var asm in assemblies)
            {
                foreach (var type in asm.GetTypes())
                {
                    var tsc = type.GetCustomAttribute<SaveComponentAttribute>();
                    if (tsc == null) continue;

                    // Validate members before generating code
                    ValidateSaveableMembers(type);

                    var targetType = type;
                    var saveableFields = type
                        .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<SaveableAttribute>() != null)
                        .ToArray();

                    var saveableProperties = type
                        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(p => p.GetCustomAttribute<SaveableAttribute>() != null)
                        .ToArray();

                    var customSerializer = type.GetCustomAttribute<UseCustomSerializerAttribute>();

                    string fileName = targetType.Name + config.FileSuffix + ".cs";
                    string filePath = Path.Combine(config.GeneratedCodeOutputPath, fileName);

                    string code = GenerateTypeCode(config.GeneratedNamespace, targetType, saveableFields,
                        saveableProperties, customSerializer, config.AdditionalNamespaces);
                    File.WriteAllText(filePath, code);
                }
            }

            AssetDatabase.Refresh();
        }
        
        private static void ValidateSaveableMembers(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<SaveableAttribute>() != null);

            foreach (var field in fields)
            {
                var publicBackingField = field.Name.Contains("__BackingField") 
                                         && (type.GetMethod($"get_{GetFieldName(field)}")?.IsPublic ?? false)
                                         && (type.GetMethod($"set_{GetFieldName(field)}")?.IsPublic ?? false);
                
                if (!field.IsPublic && !publicBackingField)
                {
                    throw new ArgumentException($"Field '{GetFieldName(field)}' in '{type.FullName}' marked with [Saveable] must be public for get and set.");
                }
            }
            
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.GetCustomAttribute<SaveableAttribute>() != null);

            foreach (var property in properties)
            {
                if (!(property.CanRead && property.GetMethod.IsPublic && property.CanWrite && property.SetMethod.IsPublic))
                {
                    
                    throw new ArgumentException($"Property '{GetPropertyName(property)}' in '{type.FullName}' marked with [Saveable] must have both a public getter and setter.");
                }
            }
        }

        private static string GetPropertyName(PropertyInfo prop)
        {
            var saveableAttribute = prop.GetCustomAttribute<SaveableAttribute>();
            var overrideName = saveableAttribute.OverrideName;
            var propName = overrideName ?? prop.Name;
            
            if (string.IsNullOrEmpty(propName))
                throw new ArgumentException("Property name cannot be null or empty.", nameof(propName));

            return propName;
        }
        
        private static string GetFieldName(FieldInfo field)
        {
            var saveableAttribute = field.GetCustomAttribute<SaveableAttribute>();
            var overrideName = saveableAttribute.OverrideName;
            var fieldName = overrideName ?? field.Name;
            
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException("Field name cannot be null or empty.", nameof(fieldName));

            var match = Regex.Match(fieldName, @"^<(.+?)>k__BackingField$");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return fieldName;
        }
        
        private static string GenerateTypeCode(
            string ns,
            Type targetType,
            FieldInfo[] saveableFields,
            PropertyInfo[] saveableProperties,
            UseCustomSerializerAttribute customSerializer,
            string[] additionalNamespaces)
        {
            string typeName = targetType.Name;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"using {nameof(SaveLoadEntitiesExtension)};");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("using System.Collections.Generic;");

            foreach (var ns1 in additionalNamespaces)
            {
                sb.AppendLine($"using {ns1};");
            }

            if (!string.IsNullOrEmpty(ns))
                sb.AppendLine($"namespace {ns} {{");

            sb.AppendLine($"public static class {typeName}_Serialization {{");

            if (customSerializer == null)
            {
                sb.AppendLine("    private class " + typeName + "Data {");
                foreach (var field in saveableFields)
                {
                    sb.AppendLine($"        public string {GetFieldName(field)};");
                }
                foreach (var prop in saveableProperties)
                {
                    sb.AppendLine($"        public string {GetPropertyName(prop)};");
                }
                sb.AppendLine("    }"); 
            }

            // Generate Save method
            sb.AppendLine($"    public static string Save_{typeName}(IComponentAdapter c, ISerializer serializer, ISaveLoadContext context) {{");

            if (customSerializer != null)
            {
                sb.AppendLine($"        return {GetFriendlyTypeName(customSerializer.SerializerType)}.Serialize(c, serializer, context);");
            }
            else
            {
                sb.AppendLine($"        var comp = c.GetRawComponent() as {targetType.FullName};");
                sb.AppendLine($"        var data = new {typeName}Data();");

                foreach (var field in saveableFields)
                {
                    sb.AppendLine($"        data.{GetFieldName(field)} = serializer.Serialize(comp.{GetFieldName(field)});");
                }
                foreach (var prop in saveableProperties)
                {
                    sb.AppendLine($"        data.{GetPropertyName(prop)} = serializer.Serialize(comp.{GetPropertyName(prop)});");
                }

                sb.AppendLine("        return serializer.Serialize(data);");
            }

            sb.AppendLine("    }");
            
            sb.AppendLine($"    public static void Load_{typeName}(IComponentAdapter c, string json, ISerializer serializer, ISaveLoadContext context) {{");

            if (customSerializer != null)
            {
                sb.AppendLine($"        {GetFriendlyTypeName(customSerializer.SerializerType)}.Deserialize(c, json, serializer, context);");
            }
            else
            {
                sb.AppendLine($"        var comp = c.GetRawComponent() as {targetType.FullName};");
                sb.AppendLine($"        var data = serializer.Deserialize<{typeName}Data>(json);");

                foreach (var field in saveableFields)
                {
                    sb.AppendLine($"        comp.{GetFieldName(field)} = serializer.Deserialize<{GetFriendlyTypeName(field.FieldType)}>(data.{GetFieldName(field)});");
                }
                foreach (var prop in saveableProperties)
                {
                    sb.AppendLine($"        comp.{GetPropertyName(prop)} = serializer.Deserialize<{GetFriendlyTypeName(prop.PropertyType)}>(data.{GetPropertyName(prop)});");
                }
            }

            sb.AppendLine("    }");

            sb.AppendLine("    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]");
            sb.AppendLine("    public static void Register() {");
            sb.AppendLine($"        SaveableComponentRegistry.Register(\"{typeName}\", Save_{typeName}, Load_{typeName});");
            sb.AppendLine("    }");

            sb.AppendLine("}");

            if (!string.IsNullOrEmpty(ns))
                sb.AppendLine("}");

            return sb.ToString();
        }

        private static string GetFriendlyTypeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.FullName!.Replace("+", ".");
            }

            var genericTypeName = type.GetGenericTypeDefinition().FullName!.Replace("+", ".");
            var genericArguments = type.GetGenericArguments();
            var friendlyNames = string.Join(", ", genericArguments.Select(GetFriendlyTypeName));
            return $"{genericTypeName![..genericTypeName.IndexOf('`')]}<{friendlyNames}>";
        }
    }
}
