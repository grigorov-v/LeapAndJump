using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Grigorov.Controllers {
    public static class ControllersRegister {
        static List<AbstractController> _allControllers = new List<AbstractController>();

        public static List<AbstractController> AllControllers {
            get {
                if ( _allControllers.Count == 0 ) {
                    var outType = typeof(AbstractController);
                    _allControllers = Assembly.GetAssembly(outType)
                                .GetTypes()
                                .Where(type => type.IsSubclassOf(outType))
                                .Select(type => Activator.CreateInstance(type) as AbstractController)
                                .ToList();
                }
                return _allControllers;
            }
        }

        public static T FindController<T>() where T: AbstractController {
            var controller = _allControllers.Find(c => c is T);
            return ( controller != null ) ? controller as T : null;
        }
    }
}