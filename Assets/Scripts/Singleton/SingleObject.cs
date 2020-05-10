namespace Grigorov.Singleton {
    public class SingleObject<T> where T : SingleObject<T>, new () {
        static T _instance;
		public static T Instance {
			get {
				if ( _instance == null ) {
					_instance = new T();
					_instance.Init();
				}
				return _instance;
			}
		}

		protected virtual void Init() { }
    }
}