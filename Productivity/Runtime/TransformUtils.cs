using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Productivity
{
    public static class TransformUtils
    {
        /// <summary>
        /// Gets all parents of this Transform in order from closest to farthest.
        /// </summary>
        /// <param name="transform">The transform to get the parents from</param>
        /// <returns>All parents from closest to farthest</returns>
        public static Transform[] GetParents(this Transform transform)
        {
            List<Transform> parents = new List<Transform>();
            Transform nextTransform = transform;

            while (nextTransform.parent != null)
            {
                nextTransform = nextTransform.parent;
                parents.Add(nextTransform);
            }

            return parents.ToArray();
        }

        /// <summary>
        /// Gets the Transforms depth in the scene hierarchy.
        /// </summary>
        /// <param name="transform">The Transform to get the hierarchy depth from</param>
        /// <returns>A number representing the depth in the hierarchy (0 at the root)</returns>
        public static int GetHierarchyDepth(this Transform transform)
        {
            int depth = 0;
            while (transform != null)
            {
                transform = transform.parent;
                ++depth;
            }

            return depth;
        }

        /// <summary>
        /// Gets all parents of this Transform in order from closest to farthest.
        /// </summary>
        /// <param name="transform">The transform to get the parents from</param>
        /// <returns>All parents from closest to farthest</returns>
        public static Transform[] GetTransformParents(Transform transform)
        {
            return transform.GetParents();
        }

        /// <summary>
        /// Gets the Transforms depth in the scene hierarchy.
        /// </summary>
        /// <param name="transform">The Transform to get the hierarchy depth from</param>
        /// <returns>A number representing the depth in the hierarchy (0 at the root)</returns>
        public static int GetTransformDepth(Transform transform)
        {
            return transform.GetHierarchyDepth();
        }

        /// <summary>
        /// Gets the closest sharing parent Transform of all the passed Transforms.
        /// </summary>
        /// <param name="transforms">Transforms to lookup the closest sharing parent</param>
        /// <returns>The closest sharing parent Transform</returns>
        public static Transform GetClosestSharingParent(Transform[] transforms)
        {
            Dictionary<Transform, int> parentOccurrences = new Dictionary<Transform, int>();

            for (int i = 0; i < transforms.Length; ++i)
            {
                Transform[] parents = transforms[i].GetParents();
                foreach (var parent in parents)
                {
                    if (!parentOccurrences.ContainsKey(parent))
                        parentOccurrences.Add(parent, 0);

                    ++parentOccurrences[parent];
                }
            }

            Transform sharingParent = null;
            int highestDepth = 0;

            foreach (var entry in parentOccurrences)
            {
                if (entry.Value == transforms.Length)
                {
                    int depth = entry.Key.GetHierarchyDepth();
                    if (depth > highestDepth)
                    {
                        sharingParent = entry.Key;
                        highestDepth = depth;
                    }
                }
            }

            return sharingParent;
        }
    }
}