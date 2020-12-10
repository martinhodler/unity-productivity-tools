using UnityEditor;
using UnityEngine.Productivity.Editor.Drawers;

namespace UnityEngine.Productivity.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class CustomInspector : UnityEditor.Editor
    {
        ButtonDrawer _buttonDrawer = new ButtonDrawer();
        
        public override void OnInspectorGUI()
        {
            _buttonDrawer.Draw(this);
        
            base.OnInspectorGUI();
        }
    }
}