using UnityEngine;

namespace Grigorov.LoadingManagement {
    public class ShellCoroutine : MonoBehaviour {
        static ShellCoroutine _instance = null;

        public static ShellCoroutine Instance {
            get {
                if ( !_instance ) {
                    _instance = MonoBehaviour.FindObjectOfType<ShellCoroutine>();
                    _instance = !_instance ? new GameObject("[ShellCoroutine]").AddComponent<ShellCoroutine>() : _instance;
                    MonoBehaviour.DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
    }
}