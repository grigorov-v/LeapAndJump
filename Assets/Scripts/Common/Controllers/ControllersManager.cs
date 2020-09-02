using UnityEngine;
using UnityEngine.SceneManagement;

using System.Reflection;
using System.Linq;

namespace Grigorov.Controllers {
    public class ControllersManager : MonoBehaviour {
        static ControllersManager _firstControllersManager = null;

        bool IsCommonManager {
            get => (_firstControllersManager == this);
        }

        void Awake() {
            if ( !_firstControllersManager ) {
                InitControllers();
                DontDestroyOnLoad(gameObject);
                _firstControllersManager = this;
            }

            AwakeControllers();
            
            var sceneName = SceneManager.GetActiveScene().name;
            gameObject.name = IsCommonManager ? "[Common_Controllers_Manager]" : $"[{sceneName}_Controllers_Manager]";

            var ourtype = typeof(AbstractController); 
            var list = Assembly.GetAssembly(ourtype).GetTypes().Where(type => type.IsSubclassOf(ourtype)).ToList();
            Debug.Log(list.Count);
        }

        void Update() {
            if ( IsCommonManager ) {
                UpdateControllers();
            }
        }

        void FixedUpdate() {
            if ( IsCommonManager ) {
                FixedUpdateControllers();
            }
        }

        void OnDestroy() {
            if ( IsCommonManager ) {
                ReinitControllers();
            }
        }

        void InitControllers() {
            foreach ( var controller in ControllersRegister.AllControllers ) {
                if ( controller is IInit ) {
                    (controller as IInit).OnInit();
                }
            }
        }

        void AwakeControllers() {
            foreach ( var controller in ControllersRegister.AllControllers ) {
                if ( controller is IAwake ) {
                    (controller as IAwake).OnAwake();
                }
            }
        }

        void UpdateControllers() {
            foreach ( var controller in ControllersRegister.AllControllers ) {
                if ( controller is IUpdate ) {
                    (controller as IUpdate).OnUpdate();
                }
            }
        }

        void FixedUpdateControllers() {
            foreach ( var controller in ControllersRegister.AllControllers ) {
                if ( controller is IFixedUpdate ) {
                    (controller as IFixedUpdate).OnFixedUpdate();
                }
            }
        }

        void ReinitControllers() {
            foreach ( var controller in ControllersRegister.AllControllers ) {
                if ( controller is IDeinit ) {
                    (controller as IDeinit).OnDeinit();
                }
            }
        }
    }
}