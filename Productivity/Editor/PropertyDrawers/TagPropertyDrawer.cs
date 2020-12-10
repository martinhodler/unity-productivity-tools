
using System;
using UnityEditor;
using UnityEngine.Productivity.Attributes;

namespace UnityEngine.Productivity.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                //Debug.LogWarning($"Tag-Attribute was used on a non-string property: {property.serializedObject.targetObject.GetType().ToString()}.{property.name}");
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            string value = property.stringValue;
            int index = Array.IndexOf(UnityEditorInternal.InternalEditorUtility.tags, value);

            if (index < 0)
                index = 0;
            
            EditorGUI.BeginProperty(position, label, property);
            
            Rect valueEditPosition = EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));
            index = EditorGUI.Popup(valueEditPosition, index, UnityEditorInternal.InternalEditorUtility.tags);

            property.stringValue = UnityEditorInternal.InternalEditorUtility.tags[index];
            
            EditorGUI.EndProperty();
        }
    }
}