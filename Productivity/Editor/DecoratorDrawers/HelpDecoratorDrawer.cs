
using UnityEditor;
using UnityEngine.Productivity.Attributes;


namespace UnityEngine.Productivity.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(HelpAttribute))]
    public class HelpDecoratorDrawer : DecoratorDrawer
    {
        private const float IconSpace = 32;
        private const float InnerPadding = 4;
        private const float MarginBottom = 2;
        private const float MarginTop = 2;

        public override void OnGUI(Rect position)
        {
            if (attribute is HelpAttribute helpAttributeAttribute)
            {
                position.y += MarginTop;
                position.height -= MarginBottom + MarginTop;
                EditorGUI.HelpBox(position, helpAttributeAttribute.Message, (MessageType) helpAttributeAttribute.Type);
            }
        }

        public override float GetHeight()
        {
            if (attribute is HelpAttribute helpAttributeAttribute)
            {
                float textWidth = (helpAttributeAttribute.Type != HelpAttribute.MessageType.None)
                    ? EditorGUIUtility.currentViewWidth - IconSpace
                    : EditorGUIUtility.currentViewWidth;


                GUIStyle style = EditorStyles.helpBox;
                return style.CalcHeight(new GUIContent(helpAttributeAttribute.Message), textWidth) + InnerPadding + MarginBottom + MarginTop;
            }

            return base.GetHeight();
        }

        public override bool CanCacheInspectorGUI()
        {
            return true;
        }
    }
}