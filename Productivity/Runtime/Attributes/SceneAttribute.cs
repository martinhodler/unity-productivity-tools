using System;

namespace UnityEngine.Productivity.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SceneAttribute : PropertyAttribute
    {
        public bool InBuildCheck { get; }

        /// <summary>
        /// Attribute to display the string as a SceneAsset in the inspector.
        /// </summary>
        /// <param name="inBuildCheck">If true the inspector shows a warning when the scene is not in the build.</param>
        public SceneAttribute(bool inBuildCheck = true)
        {
            InBuildCheck = inBuildCheck;
        }
    }
}