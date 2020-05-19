﻿namespace Grigorov.Controller {
    public class BaseController<T>: IController where T: class, IController {
        public static T Instance { get; private set; }

        public virtual void Init() {
            Instance = (Instance == null) ? this as T : Instance;
        }

        public virtual void PostInit() { 
        }

        public virtual void Reinit() { 
        }
    }
}