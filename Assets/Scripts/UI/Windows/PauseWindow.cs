using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.UI {
    public class PauseWindow : BaseWindow {
        [Space]
        [SerializeField] Button _closeButton    = null;
        [SerializeField] Button _mainMenuButton = null;
        [SerializeField] Button _restartButton  = null;

        ScenesController _sceneController = null;

        void Awake() {
            _closeButton.onClick.AddListener(OnCloseClick);
            _mainMenuButton.onClick.AddListener(OnMainMenuClick);
            _restartButton.onClick.AddListener(OnRestartClick);
            _sceneController = Grigorov.Controllers.ControllersRegister.FindController<ScenesController>();
        }

        void OnCloseClick() {
            UnPause();
            Hide();
        }

        void OnMainMenuClick() {
            UnPause();
            _sceneController?.OpenScene(Scenes.MainMenu);
        }

        void OnRestartClick() {
            UnPause();
            _sceneController?.RestartCurrentScene();
        }

        void UnPause() {
            Time.timeScale = 1;
        }
    }
}