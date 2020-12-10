using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Productivity.Editor.Utils;
using UnityEngine.SceneManagement;

namespace UnityEngine.Productivity.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(Attributes.SceneAttribute))]
    public class ScenePropertyDrawer : PropertyDrawer
    {
        private bool _sceneNotFound = false;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            
            Attributes.SceneAttribute sceneAttributeAttribute = attribute as Attributes.SceneAttribute;
            if (sceneAttributeAttribute == null)
            {
                Debug.LogWarning($"This error shouldn't happen. Unity called a CustomPropertyDrawer on a wrong type: {property.serializedObject.targetObject.GetType().ToString()}.{property.name}");
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            SceneAsset scene = null;
            
            string sceneName = property.stringValue;
            scene = GetSceneAssetByName(sceneName);

            if (!string.IsNullOrEmpty(sceneName) && scene == null)
            {
                _sceneNotFound = true;
                EditorMessage.DisplayError($"The stored scene \"{sceneName}\" was not found.", "Dismiss",
                    () => { _sceneNotFound = false; });
            }

            if (scene != null && !IsSceneInBuildPath(scene) && sceneAttributeAttribute.InBuildCheck)
            {
                EditorMessage.DisplayWarning($"The scene \"{scene.name}\" was not added to list of \"Scenes in Build\".", "Add to Build",
                    () =>
                    {
                        AddSceneToBuild(scene);
                    });
            }

            EditorGUI.BeginProperty(position, label, property);
            
            Rect valueEditPosition = EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));

            scene = (SceneAsset)EditorGUI.ObjectField(valueEditPosition, scene, typeof(SceneAsset), false);
            

            if (scene != null)
            {
                property.stringValue = scene.name;
            }
            else
            {
                
                
                if (!_sceneNotFound || Event.current.keyCode == KeyCode.Delete || Event.current.keyCode == KeyCode.Backspace)
                    property.stringValue = string.Empty;
            }

            EditorGUI.EndProperty();
        }

        private static SceneAsset GetSceneAssetByName(string sceneName)
        {
            if (!string.IsNullOrWhiteSpace(sceneName))
            {
                string[] assets = AssetDatabase.FindAssets("t:sceneAsset");
                SceneAsset[] scenes = new SceneAsset[assets.Length];
                
                for (int i = 0; i < assets.Length; ++i)
                {
                    string path = AssetDatabase.GUIDToAssetPath(assets[i]);
                    scenes[i] = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                }

                return scenes.FirstOrDefault(sceneAsset => sceneAsset.name == sceneName);
            }

            return null;
        }

        private static SceneAsset GetSceneByBuildIndex(int buildIndex)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);

            if (!string.IsNullOrEmpty(path))
                return AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

            return null;
        }

        private static bool IsSceneInBuildPath(SceneAsset sceneAsset)
        {
            string path = AssetDatabase.GetAssetPath(sceneAsset);
            return SceneUtility.GetBuildIndexByScenePath(path) != -1;
        }

        private static void AddSceneToBuild(SceneAsset sceneAsset)
        {
            string path = AssetDatabase.GetAssetPath(sceneAsset);
            
            List<EditorBuildSettingsScene> scenesInBuild =
                new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

            
            scenesInBuild.Add(new EditorBuildSettingsScene(path, true));
            EditorBuildSettings.scenes = scenesInBuild.ToArray();
        }
    }
}