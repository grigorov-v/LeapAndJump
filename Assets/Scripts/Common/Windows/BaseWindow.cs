﻿using UnityEngine;
using UnityEngine.UI;

namespace Grigorov.UI {
    [RequireComponent(typeof(GraphicRaycaster))]
    public abstract class BaseWindow : MonoBehaviour {
        [SerializeField] bool _isPopup = false;

        Canvas _canvas        = null;
        float  _lastTimeScale = 1;

        public bool IsPopup {
            get => _isPopup;
        }

        public bool IsActive {
            get => gameObject.activeSelf;
        }

        protected virtual bool PauseEnabled {
            get => false;
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
            if ( PauseEnabled ) {
                _lastTimeScale = Time.timeScale;
                Time.timeScale = 0;
            }

            if ( !IsPopup ) {
                Windows.HideAllWindows();
            }
            
            gameObject.SetActive(true);
        }

        public virtual void Hide() {
            if ( PauseEnabled ) {
                Time.timeScale = _lastTimeScale;
            }
            gameObject.SetActive(false);
        }

        void OnDestroy() {
            if ( PauseEnabled ) {
                Time.timeScale = _lastTimeScale;
            }
        }
    }
}