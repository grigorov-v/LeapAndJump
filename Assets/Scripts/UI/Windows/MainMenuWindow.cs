using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.UI {
    public class MainMenuWindow : BaseWindow {
        [Space]
        [SerializeField] Button _startGameButton = null;

        void Awake() {
            _startGameButton.onClick.AddListener(OnStartClick);
        }

        void OnStartClick() {
            ScenesController.Instance.OpenScene(Scene.World_1);
        }
    }
}