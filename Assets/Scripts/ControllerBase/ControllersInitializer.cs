using System;
using UnityEngine;

using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.Controller {
    public class ControllersInitializer : MonoBehaviour {
        static ControllersInitializer _controllersInitializer = null;

        private void Awake() {
            if ( _controllersInitializer ) {
                Destroy(gameObject);
                return;
            }

            AddControllers(
                new ScenesController()
            );

            ControllersRegister.Controllers.ForEach(controller => controller.Init());
            ControllersRegister.Controllers.ForEach(controller => controller.PostInit());
            _controllersInitializer = this;

            gameObject.name = "[ControllersInitializer]";
            DontDestroyOnLoad(gameObject);
        }

        void AddControllers(params IController[] controllers) {
            Array.ForEach(controllers, controller => ControllersRegister.AddController(controller));
        }

        void OnDestroy() {
            if ( _controllersInitializer != this ) {
                return;
            }
            ControllersRegister.Controllers.ForEach(controller => controller.Reinit());
            ControllersRegister.Controllers.Clear();
        }
    }
}