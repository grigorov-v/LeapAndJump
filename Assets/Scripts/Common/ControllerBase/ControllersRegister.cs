using System.Collections.Generic;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.Controller {
    public static class ControllersRegister {
        internal static List<IController> AllControllers { get; private set; } = new List<IController>() {
            new ScenesController()
        };

        public static T FindController<T>() where T: class, IController {
            var controller = AllControllers.Find(c => c is T);
            return (controller == null) ? null : controller as T;
        }
    }
}