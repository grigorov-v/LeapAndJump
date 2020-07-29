using UnityEngine;
using System.Collections.Generic;

namespace Grigorov.Controllers {
    public class ControllersManager : MonoBehaviour {
        static ControllersManager _controllersManager = null;

        List<IAwake>   _iAwakes  = new List<IAwake>();
        List<IStart>   _iStarts  = new List<IStart>();
        List<IUpdate>  _iUpdates = new List<IUpdate>();
        List<IDestroy> _iDestroy = new List<IDestroy>();

        void Awake() {
            if ( _controllersManager ) {
                Destroy(gameObject);
                return;
            }

            Init();
            _iAwakes.ForEach(iAwake => iAwake.OnAwake());
            
            gameObject.name = "[ControllersInitializer]";
            DontDestroyOnLoad(gameObject);
            _controllersManager = this;
        }

        void Start() {
            if ( _controllersManager != this ) {
                return;
            }

            _iStarts.ForEach(iStart => iStart.OnStart());
        }

        void Update() {
            if ( _controllersManager != this ) {
                return;
            }

            _iUpdates.ForEach(iUpdate => iUpdate.OnUpdate());
        }

        void OnDestroy() {
            if ( _controllersManager != this ) {
                return;
            }

            _iDestroy.ForEach(iDestroy => iDestroy.OnDestroy());
            ControllersRegister.AllControllers.Clear();
        }

        void Init() {
            foreach ( var pair in ControllersRegister.AllControllers ) {
                var controller = pair.Value;
                if ( controller is IAwake ) {
                    _iAwakes.Add(controller as IAwake);
                }
                if ( controller is IStart ) {
                    _iStarts.Add(controller as IStart);
                }
                if ( controller is IUpdate ) {
                    _iUpdates.Add(controller as IUpdate);
                }
                if ( controller is IDestroy ) {
                    _iDestroy.Add(controller as IDestroy);
                }
            }
        }
    }
}