namespace Modules.ComponentSerialization
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    [CreateAssetMenu(
        fileName = "SaveSystemGeneratorConfig", 
        menuName = "Save/SaveSystemGeneratorConfig")]
    public class SaveSystemGeneratorConfig : ScriptableObject
    {
        [Title("Основные настройки")]

        [Tooltip("Пространство имён, которое будет использоваться в сгенерированных файлах.")]
        public string generatedNamespace = "ProjectName.SaveGenerated";

        [FolderPath(RequireExistingPath = true), Tooltip("Папка, куда будут складываться сгенерированные .cs файлы.")]
        public string outputFolder = "Assets/Generated";

        [ToggleLeft, Tooltip("Если true, будет генерироваться отдельный файл на каждый [SaveComponent]. Если false – один общий файл.")]
        public bool generateOneFilePerClass = true;

        [Space(10)]
        [Title("Доп. настройки")]

        [ToggleLeft, Tooltip("Если включено, при генерации будет вызван AssetDatabase.Refresh() в конце.")]
        public bool autoRefresh = true;

        [ToggleLeft, Tooltip("Если включено, код будет автоматически генерироваться при изменении ассетов.")]
        public bool autoGenerateOnAssetChange = true;

        [InfoBox("Нажмите кнопку, чтобы сгенерировать код на основе найденных [SaveComponent] классов.")]
        [Button("Generate Code", ButtonSizes.Large)]
        public void GenerateCode()
        {
            SaveCodeGenerator.GenerateAllDtoAndSerializers(outputFolder, generatedNamespace, generateOneFilePerClass, autoRefresh);
        }
    }

    // AssetPostprocessor to trigger code generation on asset changes
}
