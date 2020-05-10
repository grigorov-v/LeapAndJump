using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Grigorov.Singleton;
using Grigorov.SceneManagement;

using DG.Tweening;

namespace Grigorov.LeapAndJump.UI {
    public class LoadingUI : SingleBehaviour<LoadingUI> {
        [SerializeField] Image       _bar         = null;
        [SerializeField] CanvasGroup _canvasGroup = null;

        Tween _tween = null;

        void Start() {
            if ( SceneManager.GetActiveScene().name == "Loading" ) {
                LoadSceneHelper.StartLoadingScene("MainMenu");
            }
        }

        public void UpdateBar(float progress) {
            _bar.fillAmount = progress;
        }

        public void Show(float duration) {
            KillTween();
            _canvasGroup.alpha = 0;
            gameObject.SetActive(true);
            _tween = _canvasGroup.DOFade(1, duration);
        }

        public void Hide(float duration) {
            KillTween();
            _tween = _canvasGroup.DOFade(0, duration);
            _tween.onKill += () => {
                gameObject.SetActive(false);
            };
        }

        void KillTween() {
            if ( _tween == null ) {
                return;
            }

            _tween.Kill();
            _tween = null;
        }

        public static void CreateNewObject() {
            var obj = Resources.Load<LoadingUI>("Prefabs/Loading");
            MonoBehaviour.Instantiate(obj);
            LoadingUI.UpdateInstance();
        }
    } 
}