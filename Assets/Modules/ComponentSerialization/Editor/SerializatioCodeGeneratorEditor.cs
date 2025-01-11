using Modules.ComponentSerialization;
using UnityEditor;
using UnityEngine;

public static class SerializatioCodeGeneratorEditor
{
    [MenuItem("Tools/Component Serialization/Generate Serialization Code", priority = 1)]
    private static void GenerateSaveSystemCode()
    {
       
        var config = FindConfig();
        
        if(config == null)
        {
            Debug.LogError("Save system code generation failed. No config found.");
            return;
        }
        
        config.GenerateCode();

        Debug.Log($"Save system code generation completed.");
    }
    
    [MenuItem("Tools/Component Serialization/Delete Output Folder", priority = 2)]
    private static void DeleteFolder()
    {
        var config = FindConfig();
        
        if(config == null)
        {
            Debug.LogError("Delete output folder failed. No config found.");
            return;
        }
        
        config.DeleteFolder();

        Debug.Log($"Save system code generation completed.");
    }
    
    private static SerializationCodeGeneratorConfig FindConfig()
    {
        var guids = AssetDatabase.FindAssets($"t:{nameof(SerializationCodeGeneratorConfig)}");

        if (guids.Length == 0)
        {
            Debug.LogError("No SaveSystemGeneratorConfig asset found. Please create one in your project.");
            return null;
        }

        if (guids.Length > 1)
        {
            Debug.LogWarning("Multiple SaveSystemGeneratorConfig assets found. Using the first one.");
        }

        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        var config = AssetDatabase.LoadAssetAtPath<SerializationCodeGeneratorConfig>(path);

        return config;
    }
}