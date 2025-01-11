using UnityEditor;
using UnityEngine;

namespace Modules.ComponentSerialization
{
    public class SaveSystemAssetPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(
            string[] importedAssets, 
            string[] deletedAssets, 
            string[] movedAssets, 
            string[] movedFromAssetPaths)
        {
            var configs = AssetDatabase.FindAssets("t:SaveSystemGeneratorConfig");
            foreach (var configGUID in configs)
            {
                var configPath = AssetDatabase.GUIDToAssetPath(configGUID);
                var config = AssetDatabase.LoadAssetAtPath<SaveSystemGeneratorConfig>(configPath);

                if (config != null && config.autoGenerateOnAssetChange)
                {
                    config.GenerateCode();
                }
            }
        }
    }
}