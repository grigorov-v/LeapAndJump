using Grigorov.LeapAndJump.Controllers;
using Grigorov.UI;
using Grigorov.Unity.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Grigorov.LeapAndJump.UI {
	public class MainMenuWindow : BaseWindow {
		[Space] [SerializeField] Button _startGameButton;

		ScenesController _sceneController;

		void Awake() {
			_startGameButton.onClick.AddListener(OnStartClick);
			_sceneController = ControllersBox.Get<ScenesController>();
		}

		void OnStartClick() {
			var lc = ControllersBox.Get<LevelController>();
			_sceneController?.OpenLevel(lc.CurrentLevel);
		}
	}
}