using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

namespace Grigorov.Controllers {
    public class ControllersProcessor : MonoBehaviour {
        static ControllersProcessor _firstControllersManager = null;

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
                SceneManager.sceneLoaded += OnSceneLoaded;
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
                SceneManager.sceneLoaded -= OnSceneLoaded;
            }
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if ( FindObjectsOfType<ControllersProcessor>().Count(cp => cp != _firstControllersManager) == 0 ) { 
                CreateControllersProcessor();
            }
        }

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeInitializeOnLoad() {
            if ( !FindObjectOfType<ControllersProcessor>() ) {
                CreateControllersProcessor();
            } 
        }

        static void CreateControllersProcessor() {
            new GameObject().AddComponent<ControllersProcessor>();
        }
    }
}