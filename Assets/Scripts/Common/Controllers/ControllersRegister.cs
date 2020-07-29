using System;
using System.Collections.Generic;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.Controllers {
    public static class ControllersRegister {
        internal static Dictionary<Type, object> AllControllers = new Dictionary<Type, object>() {
            { typeof(ScenesController),      new ScenesController()      },
            { typeof(FoodCollectController), new FoodCollectController() }
        };

        public static T FindController<T>() where T: class {
            object result = null;
            var type = typeof(T);
            AllControllers.TryGetValue(type, out result);
            return (T)result;
        }
    }
}