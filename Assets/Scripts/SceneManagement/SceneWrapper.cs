using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Grigorov.Extensions;

namespace Grigorov.SceneManagement {
    public class SceneWrapper {
        string         _sceneName      = null;
        AsyncOperation _asyncOperation = null;

        List<Action<Scene>> _loadedActions   = new List<Action<Scene>>();
        List<Action<Scene>> _unloadedActions = new List<Action<Scene>>();
        List<Action<float>> _loadingActions  = new List<Action<float>>();

        public SceneWrapper LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
            _sceneName = sceneName;
            _asyncOperation = SceneManager.LoadSceneAsync(_sceneName, loadSceneMode);
            return this;
        }

        public SceneWrapper LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single) {
            _sceneName = sceneName;
            SceneManager.LoadScene(_sceneName, loadSceneMode);
            return this;
        }

        public SceneWrapper UnloadScene(string sceneName) {
            _sceneName = sceneName;
            SceneManager.UnloadSceneAsync(_sceneName);
            Debug.Log("UnloadScene " + _sceneName);
            return this;
        }

        public SceneWrapper UnloadScene() {
            return UnloadScene(_sceneName);
        }

        public SceneWrapper AddLoadedAction(Action<Scene> action) {
            _loadedActions.AddIfNotExists(action);
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            return this;
        }

        public SceneWrapper AddLoadingAction(Action<float> action) {
            _loadingActions.AddIfNotExists(action);
            ShellCoroutine.Instance.StartCoroutine(LoadingCoroutine());
            return this;
        }

        public SceneWrapper AddUnloadedAction(Action<Scene> action) {
            _unloadedActions.AddIfNotExists(action);
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            return this;
        }

        IEnumerator LoadingCoroutine() {
            while ( (_asyncOperation != null) && !_asyncOperation.isDone ) {
                var progress = _asyncOperation.progress / 0.9f;
                _loadingActions.ForEach(action => action?.Invoke(progress));
                yield return null;
            }
            _loadingActions.Clear();
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) {
            if ( scene.name != _sceneName ) {
                return;
            }
            _loadedActions.ForEach(action => action?.Invoke(scene));
            SceneManager.sceneLoaded -= OnSceneLoaded;
            _loadedActions.Clear();
        }

        void OnSceneUnloaded(Scene scene) {
            if ( scene.name != _sceneName ) {
                return;
            }
            _unloadedActions.ForEach(action => action?.Invoke(scene));
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            _unloadedActions.Clear();
        }
    }
}