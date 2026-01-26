using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Escalon
{
    public static class GameObjectExtensions
    {
        public static T GetInterface<T>(this GameObject inObj) where T : class
        {
            return inObj.GetComponents<Component>().OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetInterfaces<T>(this GameObject inObj) where T : class
        {
            return inObj.GetComponents<Component>().OfType<T>();
        }

        public static IEnumerable<T> GetInterfacesInChildren<T>(this GameObject inObj) where T : class
        {
            return inObj.GetComponentsInChildren<Component>().OfType<T>();
        }

        public static T GetInterfaceInChildren<T>(this GameObject inObj) where T : class
        {
            return inObj.GetComponentsInChildren<Component>().OfType<T>().FirstOrDefault();
        }

        public static T GetInterfaceInParent<T>(this GameObject inObj) where T : class
        {
            var interfaces = inObj.GetComponentsInParent<Component>().OfType<T>();
            return interfaces.Count() > 0 ? interfaces.First() : null;
        }
    }
}
