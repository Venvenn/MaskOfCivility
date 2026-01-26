using UnityEngine;
using UnityEngine.SceneManagement;

namespace Escalon
{
    public static class SceneManagerExtensions
    {
        public static T GetComponentInChildrenInScene<T>(this UnityEngine.SceneManagement.Scene scene) where T : Component
        {
            foreach (var obj in scene.GetRootGameObjects())
            {
                var c = obj.GetComponentInChildren<T>();
                if (c != null)
                {
                    return c;
                }
            }

            return null;
        }

        public static Camera GetMainCamera()
        {
            return SceneManager.GetActiveScene().GetComponentInChildrenInScene<Camera>();
        }
    }
}
