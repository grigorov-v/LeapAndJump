using UnityEngine;

namespace Grigorov.Singleton {
	public class SingleBehaviour<T, IT> : MonoBehaviour
		where T  : SingleBehaviour<T, IT>, IT
		where IT : class
	{
		static T _instance;
		public static IT Instance {
			get {
				if ( !_instance ) {
					UpdateInstance();
				}
				return _instance;
			}
		}

		public static void UpdateInstance() {
			_instance = GameObject.FindObjectOfType<T>();
		}
		
		protected virtual void Awake() {
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}

	public class SingleBehaviour<T> : SingleBehaviour<T, T>
		where T : SingleBehaviour<T, T> {}
}