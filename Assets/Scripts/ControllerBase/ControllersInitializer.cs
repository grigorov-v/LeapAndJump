using UnityEngine;

namespace Grigorov.Controller {
    public class ControllersInitializer : MonoBehaviour {
        static ControllersInitializer _controllersInitializer = null;

        ControllersRegister _register = null;

        private void Awake() {
            if ( _controllersInitializer ) {
                Destroy(gameObject);
                return;
            }

            _register = new ControllersRegister();
            _register.CreateAllControllers();
            _register.AllControllers.ForEach(controller => controller.Init());
            _register.AllControllers.ForEach(controller => controller.PostInit());
            _controllersInitializer = this;

            gameObject.name = "[ControllersInitializer]";
            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy() {
            if ( _controllersInitializer != this ) {
                return;
            }

            _register.AllControllers.ForEach(controller => controller.Reinit());
            _register.AllControllers.Clear();
            _register = null;
        }
    }
}