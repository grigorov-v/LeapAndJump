using System.Collections.Generic;

namespace Grigorov.Controllers {
    public abstract class AbstractController {
        public List<IControlled> ControlledBehaviours { get; private set; } = new List<IControlled>();

        public void AddControlledBehaviour(IControlled controlledBehaviour) {
            ControlledBehaviours.Add(controlledBehaviour);
        }

        public void RemoveControlledBehaviour(IControlled controlledBehaviour) {
            ControlledBehaviours.Remove(controlledBehaviour);
        }
    }
}