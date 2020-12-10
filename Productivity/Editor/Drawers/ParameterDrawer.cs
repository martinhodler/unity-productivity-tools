using System;
using System.Collections.Generic;
using UnityEditor;

namespace UnityEngine.Productivity.Editor.Drawers
{
    public class ParameterDrawer
    {
        private Dictionary<Type, Action<object>> _drawers = new Dictionary<Type, Action<object>>();
        private GUIContent _currentLabel;
        private object _value;
        private Type _currentType;
        
        public ParameterDrawer()
        {
            _drawers.Add(typeof(int), IntField);
            _drawers.Add(typeof(float), FloatField);
            _drawers.Add(typeof(string), StringField);
            _drawers.Add(typeof(Vector2), Vector2Field);
            _drawers.Add(typeof(Vector3), Vector3Field);
            _drawers.Add(typeof(UnityEngine.Object), ObjectField);
        }

        public object EditorField(Type type, GUIContent label, object value)
        {
            _currentType = type;
            _currentLabel = label;
            
            object tmpValue = null;
            if (type.IsValueType)
                tmpValue = Activator.CreateInstance(type);
            
            if (value != null && value.GetType() == type)
                tmpValue = value;

            if (_drawers.ContainsKey(_currentType))
            {
                _drawers[_currentType].Invoke(tmpValue);
                return _value;
            }
            
            foreach (Type t in _drawers.Keys)
            {
                if (_currentType.IsSubclassOf(t))
                {
                    _drawers[t].Invoke(tmpValue);
                    return _value;
                }
            }

            return tmpValue;
        }
        
        
        public T EditorField<T>(GUIContent label, object value)
        {
            _currentType = typeof(T);
            _currentLabel = label;

            T tmpValue = default(T);
            if (value is T)
                tmpValue = (T)value;

            if (_drawers.ContainsKey(typeof(T)))
            {
                _drawers[typeof(T)].Invoke(tmpValue);
                return (T)_value;
            }

            return tmpValue;
        }

        public bool CanEditType(Type type)
        {
            if (_drawers.ContainsKey(type))
                return true;
            else
            {
                foreach (Type t in _drawers.Keys)
                {
                    if (type.IsSubclassOf(t))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void IntField(object value)
        {
            _value = EditorGUILayout.IntField(_currentLabel, (int) value);
        }
        
        private void FloatField(object value)
        {
            _value = EditorGUILayout.FloatField(_currentLabel, (float) value);
        }
        
        private void StringField(object value)
        {
            _value = EditorGUILayout.TextField(_currentLabel, (string) value);
        }
        
        private void Vector2Field(object value)
        {
            _value = EditorGUILayout.Vector2Field(_currentLabel, (Vector2) value);
        }
        
        private void Vector3Field(object value)
        {
            _value = EditorGUILayout.Vector3Field(_currentLabel, (Vector3) value);
        }

        private void ObjectField(object value)
        {
            _value = EditorGUILayout.ObjectField(_currentLabel, (UnityEngine.Object)value, _currentType, true);
        }
    }
    
}