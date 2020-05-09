using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grigorov.SceneManagement {
    public class SceneHandler {
        string                       _sceneName        = null;
        Action<float>                _loadingAction    = null;
        AsyncOperation               _loadingOperation = null;
        
        Action<Scene, LoadSceneMode> _loadedAction     = null;
        Action<Scene>                _unloadedAction   = null;

        public SceneHandler(string sceneName) {
            _sceneName = sceneName;
        }

        public SceneHandler LoadSceneAsync() {
            _loadingOperation = SceneManager.LoadSceneAsync(_sceneName);
            return this;
        }

        public SceneHandler LoadScene() {
            SceneManager.LoadScene(_sceneName);
            return this;
        }

        public SceneHandler SetLoadedAction(Action<UnityEngine.SceneManagement.Scene, LoadSceneMode> action) {
            _loadedAction = action;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            return this;
        }

        public SceneHandler SetLoadingAction(Action<float> action) {
            _loadingAction = action;
            ShellCoroutine.Instance.StartCoroutine(LoadingCoroutine());
            return this;
        }

        public SceneHandler SetUnloadedAction(Action<Scene> action) {
            _unloadedAction = action;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            return this;
        }

        IEnumerator LoadingCoroutine() {
            while ( (_loadingOperation != null) && !_loadingOperation.isDone ) {
                var progress = _loadingOperation.progress / 0.9f;
                _loadingAction?.Invoke(progress);
                yield return null;
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) {
            if ( scene.name != _sceneName ) {
                return;
            }

            _loadedAction?.Invoke(scene, sceneMode);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneUnloaded(Scene scene) {
            if ( scene.name != _sceneName ) {
                return;
            }

            _unloadedAction?.Invoke(scene);
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
    }
}