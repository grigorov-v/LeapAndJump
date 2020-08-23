using UnityEngine;

namespace Grigorov.Extensions {
    public static class ComponentsExtension {
        public static T GetOrFindComponent<T>(this GameObject gameObject, ref T component) where T: Component {
            if ( !component ) {
                component = gameObject.GetComponent<T>();
            }
            return component;
        }
    }
}