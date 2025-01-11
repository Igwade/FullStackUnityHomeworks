#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Modules.ComponentSerialization.Runtime;
using Modules.ComponentSerialization.Runtime.Attributes;

namespace Modules.ComponentSerialization
{
    public static partial class SerializationCodeGenerator
    {
        private static readonly Dictionary<Type, (Type dtoType, Type serializerType)> SerializersMap = new();

        private static string TypeSerializersName => typeof(TypeSerializers).FullName;

        public static void Generate(
            string outputFolder,
            string generatedNamespace,
            bool oneFilePerClass,
            bool autoRefresh
        )
        {
            var logs = new List<string>();

            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                BuildSerializersMap();

                var saveComponents = TypeCache.GetTypesWithAttribute<SaveComponentAttribute>();
                if (!saveComponents.Any())
                {
                    logs.Add("[SaveCodeGenerator] No classes with [SaveComponent].");
                }

                foreach (var compType in saveComponents)
                {
                    var codeBlock = GenerateDtoAndSerializerForComponent(
                        compType,
                        out var dtoName,
                        out var serializerName
                    );

                    if (oneFilePerClass)
                    {
                        
                        var fileText = WrapInNamespace(generatedNamespace, codeBlock);
                        var fileTextWithDeps = DefaultUsings() + fileText;
                        var filePath = Path.Combine(outputFolder, compType.Name + "Serializer.cs");
                        File.WriteAllText(filePath, fileTextWithDeps);
                        var logMessage = "[SaveCodeGenerator] Generated: " + filePath;
                        logs.Add(logMessage);
                    }
                }

                if (!oneFilePerClass && saveComponents.Any())
                {
                    var sbAll = new StringBuilder();
                    sbAll.AppendLine("// Auto-generated by SaveCodeGenerator");
                    sbAll.AppendLine();
                    sbAll.AppendLine($"namespace {generatedNamespace}");
                    sbAll.AppendLine("{");

                    foreach (var compType in saveComponents)
                    {
                        var codeBlock = GenerateDtoAndSerializerForComponent(
                            compType,
                            out _,
                            out _);
                        sbAll.AppendLine(codeBlock);
                    }

                    sbAll.AppendLine("}");
                    var outFile = Path.Combine(outputFolder, "SaveComponentsGenerated.cs");
                    File.WriteAllText(outFile, sbAll.ToString());
                    var logMessage = "[SaveCodeGenerator] Generated combined file: " + outFile;
                    logs.Add(logMessage);
                }

                if (autoRefresh)
                {
                    AssetDatabase.Refresh();
                }

                foreach (var log in logs)
                {
                    Debug.Log(log);
                }
            }
            catch (Exception e)
            {
                string errorLog = "[SaveCodeGenerator] Error: " + e;
                Debug.LogError(errorLog);
            }
        }
    }
}

#endif