using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.LoadingManagement;

namespace Grigorov.LeapAndJump.UI {
    public class MainMenuWindow : BaseWindow {
        [Space]
        [SerializeField] Button _startGameButton = null;

        void Awake() {
            _startGameButton.onClick.AddListener(OnStartClick);
        }

        void OnStartClick() {
            LoadingHelper.LoadLevel();
        }
    }
}