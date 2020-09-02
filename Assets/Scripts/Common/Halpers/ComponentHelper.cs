using UnityEngine;
using System;

namespace Grigorov.Helpers {
    public static class ComponentHelper {
        public static T GetOrFindComponent<T>(ref T component, Func<T> findFunc) where T: Component {
            if ( !component ) {
                component = findFunc();
            }
            return component;
        }
    }
}