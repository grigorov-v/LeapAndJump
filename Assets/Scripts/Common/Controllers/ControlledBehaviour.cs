using UnityEngine;
using System;
using System.Collections.Generic;

namespace Grigorov.Controllers
{
	public class ControlledBehaviour<T> : MonoBehaviour where T : ControlledBehaviour<T>
	{
		static List<T> _allObjects = new List<T>();

		protected virtual void Awake()
		{
			_allObjects.Add(this as T);
		}

		protected virtual void OnDestroy()
		{
			_allObjects.Remove(this as T);
		}

		public static void ForAll(Action<T> action)
		{
			_allObjects.ForEach(obj => action?.Invoke(obj));
		}
	}
}