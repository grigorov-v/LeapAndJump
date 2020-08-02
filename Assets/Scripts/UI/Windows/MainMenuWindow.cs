using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.Controllers;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.UI {
    public class MainMenuWindow : BaseWindow {
        [Space]
        [SerializeField] Button _startGameButton = null;

        ScenesController _sceneController = null;

        void Awake() {
            _startGameButton.onClick.AddListener(OnStartClick);
            _sceneController = ControllersRegister.FindController<ScenesController>();
        }

        void OnStartClick() {
            var lc = ControllersRegister.FindController<LevelController>();
            _sceneController?.OpenScene(lc.CurrentLevel);
        }
    }
}