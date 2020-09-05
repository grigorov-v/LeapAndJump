﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Grigorov.Controllers {
    public class ControllersManager : MonoBehaviour {
        static ControllersManager _firstControllersManager = null;

        bool IsCommonManager {
            get => (_firstControllersManager == this);
        }

        List<Controller> AllControllers {
            get => Controller.AllControllers;
        }

        void Awake() {
            if ( !_firstControllersManager ) {
                AllControllers.ForEach(controller => controller?.OnInit());
                DontDestroyOnLoad(gameObject);
                _firstControllersManager = this;
            }

            AllControllers.ForEach(controller => controller?.OnAwake());
        
            var sceneName = SceneManager.GetActiveScene().name;
            gameObject.name = IsCommonManager ? "[Common_Controllers_Manager]" : $"[{sceneName}_Controllers_Manager]";
        }

        void Update() {
            if ( IsCommonManager ) {
                AllControllers.ForEach(controller => controller?.OnUpdate());
            }
        }

        void FixedUpdate() {
            if ( IsCommonManager ) {
                AllControllers.ForEach(controller => controller?.OnFixedUpdate());
            }
        }

        void OnDestroy() {
            AllControllers.ForEach(controller => controller?.OnDestroy());
            
            if ( IsCommonManager ) {
                AllControllers.ForEach(controller => controller?.OnDeinit());
            }
        }
    }
}