using UnityEngine;
using UnityEngine.UI;

namespace Grigorov.SceneManagement.UI {
    public class LoadingUI : MonoBehaviour {
        [SerializeField] Image _bar = null;

        void Awake() {
            DontDestroyOnLoad(gameObject);
        }

        public void UpdateBar(float progress) {
            _bar.fillAmount = progress;
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
    } 
}