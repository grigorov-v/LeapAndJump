﻿using UnityEngine;
using UnityEngine.SceneManagement;

using Grigorov.Unity.Events;
using Grigorov.Controllers;
using Grigorov.SceneManagement;
using Grigorov.SceneManagement.UI;

using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.Events;

namespace Grigorov.LeapAndJump.Controllers {
    public class ScenesController : Controller {
        const string LoadingUIResource = "Prefabs/LoadingUI";
        const string WorldScenePrefix  = "World_";
        const string MainMenuScene     = "MainMenu";
        const string LoadingScene      = "Loading";

        LoadingUI _loadingUI = null;

        public bool IsActiveWorldScene {
            get => CurrentSceneName.StartsWith(WorldScenePrefix);
        }

        public string CurrentSceneName {
            get => SceneManager.GetActiveScene().name;
        }

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

        public override void OnInit() {
            if ( SceneManager.GetActiveScene().name == LoadingScene ) {
                OpenMainMenu();
            }
        }

        public void OpenScene(string scene) {
            SceneLoadingHelper.StartLoadingScene(scene, LoadingUI)
                .AddLoadedAction(_ => EventManager.Fire(new ScenesController_LoadedSceneEvent(scene)));
        }

        public void OpenLevel(LevelId level) {
            var world = level.World;
            OpenScene(world);
        }

        public void OpenMainMenu() {
            OpenScene(MainMenuScene);
        }

        public void RestartCurrentScene() {
            var curScene = SceneManager.GetActiveScene().name;
            OpenScene(curScene);
        }

        LoadingUI CreateNewLoadingUIFromResources() {
            var obj = Resources.Load<LoadingUI>(LoadingUIResource);
            return MonoBehaviour.Instantiate(obj);
        }
    }
}