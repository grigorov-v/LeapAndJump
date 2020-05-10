using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Grigorov.Singleton;
using Grigorov.LoadingManagement;

using DG.Tweening;

namespace Grigorov.LeapAndJump.UI {
    public class LoadingUI : SingleBehaviour<LoadingUI> {
        [SerializeField] Image _bar = null;

        void Start() {
            if ( SceneManager.GetActiveScene().name == "Loading" ) {
                LoadingHelper.StartLoadingScene("MainMenu");
            }
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

        public static void CreateNewObject() {
            var obj = Resources.Load<LoadingUI>("Prefabs/LoadingUI");
            MonoBehaviour.Instantiate(obj);
            LoadingUI.UpdateInstance();
            LoadingUI.Instance.Hide();
        }
    } 
}