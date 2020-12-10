using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine.Productivity.Attributes;

namespace UnityEngine.Productivity.Editor.Drawers
{
    public class ButtonDrawer
    {
        private static Dictionary<string, bool> _buttonFoldouts = new Dictionary<string, bool>();
        private static Dictionary<int, List<object>> _parameters = new Dictionary<int, List<object>>();
        private UnityEditor.Editor _editor;
        private static ParameterDrawer _parameterDrawer = new ParameterDrawer();
        
        public void Draw(UnityEditor.Editor editor)
        {
            _editor = editor;
            
            Type type = _editor.target.GetType();
            bool buttonsCreated = false;

            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                ButtonAttribute buttonAttributeAttribute = (ButtonAttribute)Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

                if (buttonAttributeAttribute != null)
                {
                    bool enabledState = GUI.enabled;
                    GUI.enabled = buttonAttributeAttribute.Availability == ButtonAvailability.Always ||
                                  (EditorApplication.isPlaying
                                      ? buttonAttributeAttribute.Availability == ButtonAvailability.Play
                                      : buttonAttributeAttribute.Availability == ButtonAvailability.Editor);

                    string buttonName = String.IsNullOrEmpty(buttonAttributeAttribute.Name)
                        ? ObjectNames.NicifyVariableName(method.Name)
                        : buttonAttributeAttribute.Name;
                    
                    ParameterInfo[] parameterInfos = method.GetParameters();

                    if (parameterInfos.Length == 0)
                        DrawButton(method, buttonName);
                    else
                        DrawButtonWithParameters(method, buttonName, parameterInfos);
                    
                    GUI.enabled = enabledState;
                    buttonsCreated = true;
                }
            }
            
            if (buttonsCreated)
                EditorGUILayout.Space(20);
        }

        private void DrawButton(MethodInfo method, string buttonName)
        {
            bool activated = GUILayout.Button(buttonName);

            if (activated)
            {
                foreach (var target in _editor.targets)
                {
                    method.Invoke(target, null);
                }
            }
        }

        private void DrawButtonWithParameters(MethodInfo method, string buttonName, ParameterInfo[] parameterInfos)
        {
            using (var buttonScope = new EditorGUILayout.VerticalScope("RL Background"))
            {
                GUI.Box(new Rect(buttonScope.rect.x, buttonScope.rect.y, buttonScope.rect.width, 20), "", "RL Header");
                //GUILayout.Label(buttonName);
                if (GetFoldoutState(buttonName))
                {
                    GUILayout.Space(3);

                    EditorGUI.indentLevel++;

                    if (!_parameters.ContainsKey(_editor.target.GetHashCode()))
                        _parameters.Add(_editor.target.GetHashCode(), new List<object>());

                    List<object> parameterValues = _parameters[_editor.target.GetHashCode()];


                    parameterValues = DrawParameters(parameterValues, parameterInfos);

                    _parameters[_editor.target.GetHashCode()] = parameterValues;


                    GUILayout.Space(3);
                    bool activated = GUILayout.Button(buttonName);

                    if (activated)
                    {
                        foreach (var target in _editor.targets)
                        {
                            method.Invoke(target, parameterValues.ToArray());
                        }
                    }

                    EditorGUI.indentLevel--;
                    GUILayout.Space(2);
                }
            }
        }

        private List<object> DrawParameters(List<object> parameterValues, ParameterInfo[] parameterInfos)
        {            
            int index = 0;
            List<object> values = new List<object>();
            
            foreach (var parameter in parameterInfos)
            {
                GUIContent label = new GUIContent(ObjectNames.NicifyVariableName(parameter.Name));
                object value = null;

                if (index < parameterValues.Count)
                    value = parameterValues[index];

                if (_parameterDrawer.CanEditType(parameter.ParameterType))
                {
                    value = _parameterDrawer.EditorField(parameter.ParameterType, label, value);
                }

                values.Add(value);
                ++index;
            }

            return values;
        }

        private static bool GetFoldoutState(string buttonName)
        {
            if (!_buttonFoldouts.ContainsKey(buttonName))
                _buttonFoldouts.Add(buttonName, false);

            GUIStyle style = new GUIStyle("foldout");
            style.padding.left = 15;
            style.margin.left = 8;

            _buttonFoldouts[buttonName] = EditorGUILayout.Foldout(_buttonFoldouts[buttonName], buttonName, style);
            return _buttonFoldouts[buttonName];
        }
    }
}