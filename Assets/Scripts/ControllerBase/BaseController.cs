namespace Grigorov.Controller {
    public abstract class BaseController<T>: IController where T: class, IController, new() {
        public static T Instance { get; private set; }

        public static T Create() {
            Instance = (Instance == null) ? new T() : Instance;
            return Instance;
        }

        public abstract void Init();

        public abstract void PostInit();

        public abstract void Reinit();
    }
}