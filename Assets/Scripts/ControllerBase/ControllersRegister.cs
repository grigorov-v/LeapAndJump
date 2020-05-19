using System.Collections.Generic;

using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.Controller {
    public class ControllersRegister {
        List<IController> _controllers = new List<IController>();

        public List<IController> AllControllers {
            get {
                return _controllers;
            }
        }

        public void CreateAllControllers() {
            AddController(new TEstController());
        }

        ControllersRegister AddController<T>(T controller) where T: IController {
            _controllers.Add(controller);
            return this;
        }
    }
}