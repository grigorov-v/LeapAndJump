using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.SceneManagement;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.UI {
    public class PauseWindow : BaseWindow {
        [Space]
        [SerializeField] Button _closeButton    = null;
        [SerializeField] Button _mainMenuButton = null;
        [SerializeField] Button _restartButton  = null;

        void Awake() {
            _closeButton.onClick.AddListener(OnCloseClick);
            _mainMenuButton.onClick.AddListener(OnMainMenuClick);
            _restartButton.onClick.AddListener(OnRestartClick);
        }

        void OnCloseClick() {
            UnPause();
            Hide();
        }

        void OnMainMenuClick() {
            UnPause();
            ScenesController.Instance.OpenScene(Scene.MainMenu);
        }

        void OnRestartClick() {
            UnPause();
            ScenesController.Instance.RestartCurrentScene();
        }

        void UnPause() {
            Time.timeScale = 1;
        }
    }
}