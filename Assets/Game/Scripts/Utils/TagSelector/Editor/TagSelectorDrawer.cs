using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utils.TagSelector.Attributes;

namespace Utils.TagSelector.Editor
{
    [CustomPropertyDrawer(typeof(TagSelectorAttribute))]
    public class TagSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text, "Use [TagSelector] with a string field.");
                return;
            }

            var tags = new List<string>(UnityEditorInternal.InternalEditorUtility.tags);
            var currentIndex = tags.IndexOf(property.stringValue);

            var newIndex = EditorGUI.Popup(position, label.text, currentIndex, tags.ToArray());

            if (newIndex >= 0 && newIndex < tags.Count)
            {
                property.stringValue = tags[newIndex];
            }
        }
    }
}