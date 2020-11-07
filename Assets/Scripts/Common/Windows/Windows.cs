using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grigorov.UI
{
	public static class Windows
	{
		static List<BaseWindow> _allWindows = new List<BaseWindow>();

		static int NewSortingOrder
		{
			get
			{
				var maxOrder = 0;
				foreach (var win in _allWindows)
				{
					if (win.IsActive && (win.Canvas.sortingOrder > maxOrder))
					{
						maxOrder = win.Canvas.sortingOrder;
					}
				}
				return maxOrder + 1;
			}
		}

		public static void HideAllWindows()
		{
			_allWindows.ForEach(win => win.Hide());
		}

		public static T Get<T>() where T : BaseWindow
		{
			TryFindAllWindows();

			var window = _allWindows.Find(win => win is T);
			if (!window)
			{
				Debug.LogErrorFormat("Not found window {0}", typeof(T).ToString());
				return null;
			}

			return window as T;
		}

		static void TryFindAllWindows()
		{
			if (_allWindows.Count > 0)
			{
				return;
			}

			SceneManager.sceneUnloaded -= ClearWindowsList;
			SceneManager.sceneUnloaded += ClearWindowsList;

			var canvases = GameObject.FindObjectsOfType<Canvas>();
			foreach (var canvas in canvases)
			{
				var windows = canvas.GetComponentsInChildren<BaseWindow>(true);
				Array.ForEach(windows, win => _allWindows.Add(win));
			}
		}

		static void ClearWindowsList(Scene scene)
		{
			_allWindows.Clear();
			SceneManager.sceneUnloaded -= ClearWindowsList;
		}
	}
}