using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grigorov.SceneManagement {
    public class SceneHandler {
        string         _sceneName      = null;
        Action<float>  _loadingAction  = null;
        AsyncOperation _asyncOperation = null;
        Action<Scene>  _loadedAction   = null;
        Action<Scene>  _unloadedAction = null;

        public SceneHandler LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
            _sceneName = sceneName;
            _asyncOperation = SceneManager.LoadSceneAsync(_sceneName, loadSceneMode);
            return this;
        }

        public SceneHandler LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
            _sceneName = sceneName;
            SceneManager.LoadScene(_sceneName, loadSceneMode);
            return this;
        }

        public SceneHandler UnloadScene(string sceneName) {
            _sceneName = sceneName;
            SceneManager.UnloadSceneAsync(_sceneName);
            Debug.Log("UnloadScene " + _sceneName);
            return this;
        }

        public SceneHandler UnloadScene() {
            return UnloadScene(_sceneName);
        }

        public SceneHandler SetLoadedAction(Action<Scene> action) {
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
            while ( (_asyncOperation != null) && !_asyncOperation.isDone ) {
                var progress = _asyncOperation.progress / 0.9f;
                _loadingAction?.Invoke(progress);
                yield return null;
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) {
            if ( scene.name != _sceneName ) {
                return;
            }

            _loadedAction?.Invoke(scene);
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