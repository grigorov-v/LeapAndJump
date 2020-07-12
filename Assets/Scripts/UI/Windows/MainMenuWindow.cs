using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.Controller;
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
            _sceneController?.OpenScene(Scenes.World_1);
        }
    }
}