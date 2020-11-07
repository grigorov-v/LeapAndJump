using UnityEngine;
using System.Collections.Generic;

namespace Grigorov.Extensions
{
	public static class ComponentExtension
	{
		public static T GetComponent<T>(this Component component, ref T cache) where T : Component
		{
			if (cache)
			{
				return cache;
			}

			if (component.TryGetComponent<T>(out cache))
			{
				return cache;
			}

			return null;
		}

		public static T GetComponentInChildren<T>(this Component component, ref T cache) where T : Component
		{
			if (cache)
			{
				return cache;
			}
			cache = component.GetComponentInChildren<T>();
			return cache;
		}

		public static T GetComponentInParent<T>(this Component component, ref T cache) where T : Component
		{
			if (cache)
			{
				return cache;
			}
			cache = component.GetComponentInParent<T>();
			return cache;
		}

		public static List<T> GetComponents<T>(this Component component, ref List<T> cache) where T : Component
		{
			if ((cache != null) && (cache.Count > 0))
			{
				return cache;
			}
			component.GetComponents<T>(cache);
			return cache;
		}

		public static List<T> GetComponentsInChildren<T>(this Component component, ref List<T> cache, bool includeInactive = false) where T : Component
		{
			if ((cache != null) && (cache.Count > 0))
			{
				return cache;
			}
			component.GetComponentsInChildren<T>(includeInactive, cache);
			return cache;
		}

		public static List<T> GetComponentsInParent<T>(this Component component, ref List<T> cache, bool includeInactive = false) where T : Component
		{
			if ((cache != null) && (cache.Count > 0))
			{
				return cache;
			}
			component.GetComponentsInParent<T>(includeInactive, cache);
			return cache;
		}
	}
}