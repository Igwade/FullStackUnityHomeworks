using Sirenix.OdinInspector;
using UnityEngine;

namespace SaveLoadEntitiesExtension
{
    [CreateAssetMenu(fileName = "SaveLoadGenerationConfig", menuName = "SaveLoad/GenerationConfig")]
    public sealed class SaveLoadGenerationConfig : ScriptableObject
    {
        [Tooltip("Directory (relative to project) where generated code will be placed.")]
        public string GeneratedCodeOutputPath = "Assets/Generated/SaveLoad/";

        [Tooltip("Generated file suffix")]
        public string FileSuffix = "_Serialization";

        [Tooltip("Namespace for generated code")]
        public string GeneratedNamespace = "GeneratedSaveLoad";

        [Tooltip("Assemblies to scan for TypeSerializationConfig attributes.")]
        public string[] AssembliesToScan = { "Assembly-CSharp" };

        [Tooltip("Additional namespaces to include in generated code.")]
        public string[] AdditionalNamespaces = { };

        [Tooltip("If true, the code generator will run automatically on compilation.")]
        public bool RunOnCompile = true;

        [Button]
        public void GenerateEntitiesSerializationCode()
        {
            SaveLoadCodeGenerator.GenerateCode(this);
            Debug.Log("Successfully generated serialization code for entities.");
        }
    }

}