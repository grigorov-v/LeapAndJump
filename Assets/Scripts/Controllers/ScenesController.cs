﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System;

using Grigorov.Controllers;
using Grigorov.Events;
using Grigorov.SceneManagement;
using Grigorov.SceneManagement.UI;

using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public struct LoadedSceneEvent {
        public Scenes Scene;
        
        public LoadedSceneEvent(Scenes scene) {
            Scene = scene;
        }
    }

    public enum Scenes {
        Loading,
        MainMenu,
        World_1
    }

    public class ScenesController : Controller, IInit {
        const string LoadingUIResource = "Prefabs/LoadingUI";
        const string WorldScenePrefix  = "World_";

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

        void IInit.OnInit() {
            if ( SceneManager.GetActiveScene().name == "Loading" ) {
                OpenScene(Scenes.MainMenu);
            }
        }

        public void OpenScene(Scenes scene) {
            var sceneName = scene.ToString();
            SceneLoadingHelper.StartLoadingScene(sceneName, LoadingUI)
                .AddLoadedAction(_ => EventManager.Fire(new LoadedSceneEvent(scene)));
        }

        public void OpenScene(LevelId level) {
            var world = level.World;
            var scene = SceneEnumFromString(world);
            OpenScene(scene);
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