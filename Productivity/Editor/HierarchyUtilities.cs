using System;
using UnityEditor;
using UnityEditor.ShortcutManagement;

namespace UnityEngine.Productivity.Editor
{
    public static class HierarchyUtilities
    {
        [MenuItem("Edit/Group %g", false, 125)]
        [Shortcut("Productivity/Edit/Group", KeyCode.G, ShortcutModifiers.Action)]
        private static void GroupMenuItem()
        {
            Group();
        }

        private static void Group()
        {
            if (Selection.transforms.Length <= 1)
                return;
            
            Transform[] transforms = Selection.transforms;
            Transform parent = TransformUtils.GetClosestSharingParent(transforms);
            GameObject groupObject = new GameObject("Group");
            
            Vector3[] positions = new Vector3[transforms.Length];
            int lowestSiblingIndex = Int32.MaxValue;

            for (int i = 0; i < transforms.Length; ++i)
            {
                positions[i] = transforms[i].position;
                
                if (transforms[i].parent == parent)
                {
                    int siblingIndex = transforms[i].GetSiblingIndex();
                    if (siblingIndex < lowestSiblingIndex)
                        lowestSiblingIndex = siblingIndex;
                }
            }
            
            Bounds bounds = GeometryUtility.CalculateBounds(positions, groupObject.transform.localToWorldMatrix);
            groupObject.transform.position = bounds.center;
            groupObject.transform.SetParent(parent);
            groupObject.transform.SetSiblingIndex(lowestSiblingIndex);
            
            Undo.RegisterCreatedObjectUndo(groupObject, "group objects");
            
            foreach (var transform in transforms)
            {
                Undo.SetTransformParent(transform, groupObject.transform, "group objects");
            }
        }

        [MenuItem("Edit/Group %g", true, 125)]
        private static bool ValidateGroup()
        {
            return Selection.transforms.Length > 1;
        }
    }
}
