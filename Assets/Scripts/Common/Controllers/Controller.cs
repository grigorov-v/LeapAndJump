using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Grigorov.Extensions;

namespace Grigorov.Controllers {
    public abstract class Controller {
        static List<Controller> _allControllers = null;

        public static List<Controller> AllControllers {
            get {
                if ( _allControllers.IsNullOrEmpty() ) {
                    _allControllers = CreateAllControllers();
                }
                return _allControllers;
            }
        }

        public virtual void OnInit() {}

        public virtual void OnAwake() {}

        public virtual void OnUpdate() {}

        public virtual void OnFixedUpdate() {}

        public virtual void OnDeinit() {}

        public virtual void OnDestroy() {}

        public static T Get<T>() where T: Controller {
            var controller = AllControllers.Find(c => c is T);
            return ( controller != null ) ? controller as T : null;
        }

        static List<Controller> CreateAllControllers() {
            var outType = typeof(Controller);
            return Assembly.GetAssembly(outType).GetTypes()
                    .Where(type => type.IsSubclassOf(outType))
                    .Select(type => Activator.CreateInstance(type) as Controller)
                    .ToList();
        }
    }
}