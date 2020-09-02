using UnityEngine;

namespace Grigorov.Controllers {
    public abstract class ControlledBehaviour<T> : MonoBehaviour, IControlled where T: AbstractController {
        T Controller {
            get => ControllersRegister.FindController<T>();
        }

        protected virtual void Awake() {
            Controller.AddControlledBehaviour(this);
        }

        protected virtual void OnDestroy() {
            Controller.RemoveControlledBehaviour(this);
        }
    }
}