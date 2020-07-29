using UnityEngine;
using UnityEngine.SceneManagement;
using System;

using Grigorov.Controllers;
using Grigorov.Events;
using Grigorov.SceneManagement;
using Grigorov.SceneManagement.UI;
using Grigorov.LeapAndJump.Events;

namespace Grigorov.LeapAndJump.Controllers {
    public enum Scenes {
        Loading,
        MainMenu,
        World_1
    }

    public class ScenesController : IInit {
        const string LoadingUIResource = "Prefabs/LoadingUI";

        LoadingUI _loadingUI = null;

        LoadingUI LoadingUI {
            get {
                if ( !_loadingUI ) {
                    _loadingUI = MonoBehaviour.FindObjectOfType<LoadingUI>();
                }
                if ( !_loadingUI ) {
                    _loadingUI = CreateNewLoadingUIFromResources();
                }

                return _loadingUI;
            }
        }

        void IInit.OnInit() {
            if ( SceneManager.GetActiveScene().name == "Loading" ) {
                OpenScene(Scenes.MainMenu);
            }
        }

        public void OpenScene(Scenes scene) {
            var sceneName = scene.ToString();
            SceneLoadingHelper.StartLoadingScene(sceneName, LoadingUI)
                .AddLoadedAction(_ => EventManager.Fire(new LoadedScene(scene)));
        }

        public void RestartCurrentScene() {
            var curScene = SceneManager.GetActiveScene().name;
            var scene = SceneEnumFromString(curScene);
            OpenScene(scene);
        }

        LoadingUI CreateNewLoadingUIFromResources() {
            var obj = Resources.Load<LoadingUI>(LoadingUIResource);
            return MonoBehaviour.Instantiate(obj);
        }

        Scenes SceneEnumFromString(string sceneName) {
            return (Scenes)Enum.Parse(typeof(Scenes), sceneName);
        }
    }
}