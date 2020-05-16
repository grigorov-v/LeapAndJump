using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grigorov.UI {
    public static class Windows  {
        static List<BaseWindow> _allWindows = new List<BaseWindow>();

        static int NewSortingOrder {
            get {
                var maxOrder = 0; 
                foreach ( var win in _allWindows ) {
                    if ( win.IsActive && (win.Canvas.sortingOrder > maxOrder) ) {
                        maxOrder = win.Canvas.sortingOrder;
                    }
                }
                return maxOrder + 1;
            }
        }

        public static void ShowWindow<T>() where T: BaseWindow {
            var window = FindWindow<T>();
            if ( !window ) {
                return;
            }

            ShowWindow(window);
        }

        public static void HideWindow<T>() where T: BaseWindow {
            var window = FindWindow<T>();
            if ( !window ) {
                return;
            }

            HideWindow(window);
        }

        public static void TakeWindow<T>() where T: BaseWindow {
            var window = FindWindow<T>();
            if ( !window ) {
                return;
            }

            if ( window.IsActive ) {
                HideWindow(window);
            } else {
                ShowWindow(window);
            }
        }

        public static void HideAllWindows() {
            _allWindows.ForEach(win => win.Hide());
        } 

        public static BaseWindow FindWindow<T>() where T: BaseWindow {
            TryFindAllWindows();
            var window = _allWindows.Find(win => win is T);
            if ( !window ) {
                Debug.LogErrorFormat("Not found window {0}", typeof(T).ToString());
                return null;
            }

            return window;
        }

        static void ShowWindow(BaseWindow window) {
            if ( !window.IsPopup ) {
                HideAllWindows();
            } else {
                window.Canvas.sortingOrder = NewSortingOrder;
            }

            window.Show();
        }

        static void HideWindow(BaseWindow window) {
            window.Hide();
        }

        static void TryFindAllWindows() {
            if ( _allWindows.Count > 0 ) {
                return;
            }

            SceneManager.sceneUnloaded -= ClearWindowsList;
            SceneManager.sceneUnloaded += ClearWindowsList;

            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach ( var rootObject in rootObjects ) {
                var windows = rootObject.GetComponentsInChildren<BaseWindow>(true);
                foreach ( var win in windows ) {
                    _allWindows.Add(win);
                }
            }
        }

        static void ClearWindowsList(Scene scene) {
            _allWindows.Clear();
            SceneManager.sceneUnloaded -= ClearWindowsList;
        }
    }
}