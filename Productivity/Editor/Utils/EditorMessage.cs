using System;
using UnityEditor;

namespace UnityEngine.Productivity.Editor.Utils
{
    public static class EditorMessage
    {
        private const int HelpBoxIconSize = 24;
        private const int HelpBoxIconSpace = 32;
        private const int HelpBoxButtonMargin = 4;
        private const int InspectorDefaultPadding = 4;
        
        public static void Display(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.None);
        }
        
        public static void DisplayInfo(string infoMessage)
        {
            EditorGUILayout.HelpBox(infoMessage, MessageType.Error);
        }
        
        public static void DisplayError(string errorMessage)
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
        }
        
        public static void DisplayWarning(string warningMessage)
        {
            EditorGUILayout.HelpBox(warningMessage, MessageType.Warning);
        }
        
        public static void Display(string message, string actionName, Action actionCallback)
        {
            DisplayMessage(message, actionName, actionCallback, MessageType.None);
        }
        
        public static void DisplayInfo(string infoMessage, string actionName, Action actionCallback)
        {
            DisplayMessage(infoMessage, actionName, actionCallback, MessageType.Info);
        }
        
        public static void DisplayError(string errorMessage, string actionName, Action actionCallback)
        {
            DisplayMessage(errorMessage, actionName, actionCallback, MessageType.Error);
        }
        
        public static void DisplayWarning(string warningMessage, string actionName, Action actionCallback)
        {
            DisplayMessage(warningMessage, actionName, actionCallback, MessageType.Warning);
        }
        
        
        public static void DisplayMessage(string message, string actionName, Action actionCallback, MessageType messageType)
        {
            string iconStyle = GetIconStyleForMessageType(messageType);
            bool showIcon = !string.IsNullOrEmpty(iconStyle);

            float textWidth = (showIcon) ? EditorGUIUtility.currentViewWidth - HelpBoxIconSpace : EditorGUIUtility.currentViewWidth;
            
            using (var messageScope = new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUIContent content = new GUIContent(message);
                GUIStyle style = EditorStyles.helpBox;
                
                float height = style.CalcHeight(content, textWidth);

                if (showIcon)
                {
                    GUI.Box(
                        new Rect(messageScope.rect.x + InspectorDefaultPadding,
                            messageScope.rect.y + InspectorDefaultPadding, HelpBoxIconSize, HelpBoxIconSize),
                        GUIContent.none, iconStyle);
                    
                    height = Mathf.Max(HelpBoxIconSpace + HelpBoxButtonMargin, height);
                }

                
                Rect contentRect = EditorGUILayout.GetControlRect(false, height);

                if (showIcon)
                {
                    contentRect.x += HelpBoxIconSpace;
                    contentRect.width -= HelpBoxIconSpace;
                }

                EditorGUI.LabelField(contentRect, string.Empty, message, EditorStyles.miniLabel);
                if (GUILayout.Button(actionName))
                {
                    actionCallback.Invoke();
                }
            }
        }

        private static string GetIconStyleForMessageType(MessageType messageType)
        {
            switch (messageType)
            {
                case MessageType.Warning:
                    return "CN EntryWarnIcon";
                case MessageType.Error:
                    return "CN EntryErrorIcon";
                case MessageType.Info:
                    return "CN EntryInfoIcon";
                default:
                    return null;
            }
        }
    }
}