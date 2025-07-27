using UnityEngine;

namespace Utils
{
    public class GOUtils
    {
        /// <summary>
        /// Destroys all immediate children of the given GameObject.
        /// </summary>
        /// <param name="parent">The GameObject whose children will be destroyed.</param>
        public static void DestroyAllChildren(GameObject parent)
        {
            if (parent == null)
                return;

            foreach (Transform child in parent.transform)
            {
                Object.Destroy(child.gameObject); // Use DestroyImmediate if needed in editor scripts
            }
        }
    }
}