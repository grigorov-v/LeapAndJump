using UnityEngine;
using UnityEngine.UI;

namespace Grigorov.UI {
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class BaseWindow : MonoBehaviour {
        [SerializeField] protected bool _isPopup = false;

        Canvas _canvas = null;

        public bool IsPopup {
            get {
                return _isPopup;
            }
        }

        public bool IsActive {
            get {
                return gameObject.activeSelf;
            }
        }

        public Canvas Canvas {
            get {
                if ( !_canvas ) {
                    _canvas = GetComponent<Canvas>();
                }
                return _canvas;
            }
        }

        void OnValidate() {
           Canvas.overrideSorting = true;
        }

        public virtual void Show() {
            gameObject.SetActive(true);
        }

        public virtual void Hide() {
            gameObject.SetActive(false);
        }
    }
}