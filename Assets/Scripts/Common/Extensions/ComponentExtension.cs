using System.Collections.Generic;
using UnityEngine;

namespace Grigorov.Extensions {
	public static class ComponentExtension {
		public static T GetComponent<T>(this Component component, ref T cache) where T : Component {
			if ( cache ) {
				return cache;
			}

			return component.TryGetComponent(out cache) ? cache : null;
		}

		public static T GetComponentInChildren<T>(this Component component, ref T cache) where T : Component {
			if ( cache ) {
				return cache;
			}

			cache = component.GetComponentInChildren<T>();
			return cache;
		}

		public static T GetComponentInParent<T>(this Component component, ref T cache) where T : Component {
			if ( cache ) {
				return cache;
			}

			cache = component.GetComponentInParent<T>();
			return cache;
		}

		public static List<T> GetComponents<T>(this Component component, ref List<T> cache) where T : Component {
			if ( cache != null && cache.Count > 0 ) {
				return cache;
			}

			component.GetComponents(cache);
			return cache;
		}

		public static List<T> GetComponentsInChildren<T>(this Component component, ref List<T> cache,
			bool includeInactive = false) where T : Component {
			if ( cache != null && cache.Count > 0 ) {
				return cache;
			}

			component.GetComponentsInChildren(includeInactive, cache);
			return cache;
		}

		public static List<T> GetComponentsInParent<T>(this Component component, ref List<T> cache,
			bool includeInactive = false) where T : Component {
			if ( cache != null && cache.Count > 0 ) {
				return cache;
			}

			component.GetComponentsInParent(includeInactive, cache);
			return cache;
		}
	}
}