using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Grigorov.Controller;
using Grigorov.SceneManagement;
using Grigorov.SceneManagement.UI;

namespace Grigorov.LeapAndJump.Controllers {
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
                OpenMainMenu();
            }
            Debug.LogFormat("{0} Init", typeof(ScenesController).ToString());
        }

        public override void PostInit() {
            Debug.LogFormat("{0} PostInit", typeof(ScenesController).ToString());
        }

        public override void Reinit() {
            Debug.LogFormat("{0} Reinit", typeof(ScenesController).ToString());
        }

        public void OpenMainMenu() {
            var sceneName = "MainMenu";
            SceneLoadingHelper.StartLoadingScene(sceneName, LoadingUI);
        }

        public void OpenLevel() {
            var sceneName = "World_1";
            SceneLoadingHelper.StartLoadingScene(sceneName, LoadingUI);
        }

        public void RestartCurrentScene() {
            var curScene = SceneManager.GetActiveScene().name;
            SceneLoadingHelper.StartLoadingScene(curScene, LoadingUI);
        }

        LoadingUI CreateNewLoadingUIFromResources() {
            var obj = Resources.Load<LoadingUI>("Prefabs/LoadingUI");
            return MonoBehaviour.Instantiate(obj);
        }
    }
}