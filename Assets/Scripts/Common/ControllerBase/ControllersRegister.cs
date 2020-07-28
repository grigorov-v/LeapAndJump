using System.Collections.Generic;

namespace Grigorov.Controller {
    public static class ControllersRegister {
        public static List<IController> Controllers { get; private set; } = new List<IController>();

        public static void AddController<T>(T controller) where T: IController {
            Controllers.Add(controller);
        }

        public static T FindController<T>() where T: class, IController {
            var controller = Controllers.Find(c => c is T);
            return (controller == null) ? null : controller as T;
        }
    }
}