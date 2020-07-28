using UnityEngine;

namespace Grigorov.Controller {
    public class ControllersInitializer : MonoBehaviour {
        static ControllersInitializer _controllersInitializer = null;

        private void Awake() {
            if ( _controllersInitializer ) {
                Destroy(gameObject);
                return;
            }

            ControllersRegister.AllControllers.ForEach(controller => controller.Init());
            ControllersRegister.AllControllers.ForEach(controller => controller.PostInit());
            _controllersInitializer = this;

            gameObject.name = "[ControllersInitializer]";
            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy() {
            if ( _controllersInitializer != this ) {
                return;
            }
            ControllersRegister.AllControllers.ForEach(controller => controller.Reinit());
            ControllersRegister.AllControllers.Clear();
        }
    }
}