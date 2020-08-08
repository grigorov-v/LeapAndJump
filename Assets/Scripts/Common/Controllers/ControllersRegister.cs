using System;
using System.Collections.Generic;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.Controllers {
    struct ControllersContainer {
        public Type Key;
        public object Controller;
        public List<Type> Dependencies;

        public ControllersContainer(Type key, object controller, List<Type> dependencies = null) {
            Key = key;
            Controller = controller;
            Dependencies = dependencies;
        }
    }

    public static class ControllersRegister {
        static List<ControllersContainer> _controllers = new List<ControllersContainer>() {
            new ControllersContainer(typeof(ScenesController),        new ScenesController()),
            new ControllersContainer(typeof(BalanceController),       new BalanceController()),

            new ControllersContainer(typeof(FoodCollectController),   new FoodCollectController(), dependencies: new List<Type>() {
                typeof(BalanceController),
                typeof(LevelController),
            }),

            new ControllersContainer(typeof(LevelController),         new LevelController()),
            
            new ControllersContainer(typeof(LevelGenerateController), new LevelGenerateController(), dependencies: new List<Type>() {
                typeof(ScenesController),
                typeof(BalanceController),
                typeof(LevelController),
            }),
            
            new ControllersContainer(typeof(PlayerController), new PlayerController())
        };

        static Dictionary<Type, object> _allControllers = new Dictionary<Type, object>();

        public static Dictionary<Type, object> AllControllers {
            get {
                if ( _allControllers.Count == 0 ) {
                    foreach ( var container in _controllers ) {
                        if ( container.Dependencies != null ) {
                            foreach ( var key in container.Dependencies ) {
                                var value = _controllers.Find(c => c.Key == key).Controller;
                                TryAddController(key, value);
                            }
                        }
                        TryAddController(container.Key, container.Controller);
                    }
                }
                return _allControllers;
            }
        }

        public static T FindController<T>() where T: class {
            object result = null;
            var type = typeof(T);
            AllControllers.TryGetValue(type, out result);
            return (T)result;
        }

        static void TryAddController(Type key, object controller) {
            if ( _allControllers.ContainsKey(key) ) {
                return;
            }

            if ( controller == null ) {
                return;
            }

            _allControllers[key] = controller;
        }
    }
}