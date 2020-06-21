using UnityEngine;
using UnityEngine.SceneManagement;

using Grigorov.Controller;
using Grigorov.SceneManagement;
using Grigorov.SceneManagement.UI;

namespace Grigorov.LeapAndJump.Controllers {
    public enum Scene {
        Loading,
        MainMenu,
        World_1
    }

    public class ScenesController : BaseController<ScenesController> {
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

        public override void Init() {
            if ( SceneManager.GetActiveScene().name == "Loading" ) {
                OpenScene(Scene.MainMenu);
            }
            Debug.LogFormat("{0} Init", typeof(ScenesController).ToString());
        }

        public override void PostInit() {
            Debug.LogFormat("{0} PostInit", typeof(ScenesController).ToString());
        }

        public override void Reinit() {
            Debug.LogFormat("{0} Reinit", typeof(ScenesController).ToString());
        }

        public void OpenScene(Scene scene) {
            var sceneName = scene.ToString();
            SceneLoadingHelper.StartLoadingScene(sceneName, LoadingUI).AddLoadedAction(OnSceneLoaded);
        }

        public void RestartCurrentScene() {
            var curScene = SceneManager.GetActiveScene().name;
            SceneLoadingHelper.StartLoadingScene(curScene, LoadingUI).AddLoadedAction(OnSceneLoaded);
        }

        LoadingUI CreateNewLoadingUIFromResources() {
            var obj = Resources.Load<LoadingUI>("Prefabs/LoadingUI");
            return MonoBehaviour.Instantiate(obj);
        }

        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene) {
            Debug.LogFormat("Scene loaded {0}", scene.name);
        }
    }
}