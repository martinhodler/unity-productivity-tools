
using System.Globalization;
using UnityEditor;
using UnityEngine.Productivity.Attributes;

namespace UnityEngine.Productivity.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
    public class MinMaxRangePropertyDrawer : PropertyDrawer
    {
        private const float Space = 10f;
        private const float MinMaxLabelHeight = 12f;
        private const float DefaultPadding = 2f;

        private static readonly GUIStyle MinLabel = new GUIStyle()
        {
            alignment = TextAnchor.LowerLeft,
            normal = new GUIStyleState()
            {
                textColor = Color.gray
            }
        };
        
        private static readonly GUIStyle MaxLabel = new GUIStyle()
        {
            alignment = TextAnchor.LowerRight,
            normal = new GUIStyleState()
            {
                textColor = Color.gray
            }
        };


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Vector2)
            {
                //Debug.LogWarning($"Tag-Attribute was used on a non-Vector2 property: {property.serializedObject.targetObject.GetType().ToString()}.{property.name}");
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            
            MinMaxRangeAttribute minMaxRangeAttribute = attribute as MinMaxRangeAttribute;
            if (minMaxRangeAttribute == null)
            {
                Debug.LogWarning($"This error shouldn't happen. Unity called a CustomPropertyDrawer on a wrong type: {property.serializedObject.targetObject.GetType().ToString()}.{property.name}");
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            DrawMinMaxLabels(minMaxRangeAttribute);
            position.y += MinMaxLabelHeight;
            
            Vector2 value = property.vector2Value;
            EditorGUI.BeginProperty(position, label, property);
            
            Rect valueEditPosition = EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));
            EditorGUI.MinMaxSlider(valueEditPosition, ref value.x, ref value.y, minMaxRangeAttribute.Min, minMaxRangeAttribute.Max);
            
            EditorGUI.EndProperty();

            DrawFloatFields(ref value);

            float stepSize = minMaxRangeAttribute.StepSize;
            if (stepSize == 0)
                stepSize = 1;
            
            value.x = Mathf.Floor(value.x / stepSize) * stepSize;
            value.y = Mathf.Floor(value.y / stepSize) * stepSize;
            
            property.vector2Value = value;
        }

        private static void DrawMinMaxLabels(MinMaxRangeAttribute minMaxRangeAttribute)
        {
            Rect minMaxLabelRect = EditorGUILayout.GetControlRect(true, MinMaxLabelHeight);
            minMaxLabelRect.y -= EditorGUIUtility.singleLineHeight - 2;
            minMaxLabelRect.x += EditorGUIUtility.labelWidth + DefaultPadding;
            minMaxLabelRect.width -= EditorGUIUtility.labelWidth + DefaultPadding;

            minMaxLabelRect.width *= 0.5f;
            minMaxLabelRect.width -= Space * 0.5f;


            EditorGUI.LabelField(minMaxLabelRect, minMaxRangeAttribute.Min.ToString(CultureInfo.InvariantCulture), MinLabel);
            minMaxLabelRect.x += minMaxLabelRect.width + Space;
            EditorGUI.LabelField(minMaxLabelRect, minMaxRangeAttribute.Max.ToString(CultureInfo.InvariantCulture), MaxLabel);
        }

        private void DrawFloatFields(ref Vector2 value)
        {
            Rect controlRect = EditorGUILayout.GetControlRect(true);
            controlRect.x += EditorGUIUtility.labelWidth + DefaultPadding;
            controlRect.width -= EditorGUIUtility.labelWidth + DefaultPadding;
            
            controlRect.width *= 0.5f;
            controlRect.width -= Space * 0.5f;
            
            value.x = EditorGUI.FloatField(controlRect, value.x);
            controlRect.x += controlRect.width + Space;
            value.y = EditorGUI.FloatField(controlRect, value.y);
        }
    }
}