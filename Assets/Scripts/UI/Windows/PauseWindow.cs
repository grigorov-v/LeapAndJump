using Grigorov.LeapAndJump.Controllers;
using Grigorov.UI;
using Grigorov.Unity.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Grigorov.LeapAndJump.UI {
	public class PauseWindow : BaseWindow {
		[Space] [SerializeField] Button _closeButton;
		[SerializeField] Button _mainMenuButton;
		[SerializeField] Button _restartButton;

		ScenesController _sceneController;

		protected override bool PauseEnabled => true;

		void Awake() {
			_closeButton.onClick.AddListener(OnCloseClick);
			_mainMenuButton.onClick.AddListener(OnMainMenuClick);
			_restartButton.onClick.AddListener(OnRestartClick);
			_sceneController = ControllersBox.Get<ScenesController>();
		}

		void OnCloseClick() {
			Hide();
		}

		void OnMainMenuClick() {
			_sceneController?.OpenMainMenu();
		}

		void OnRestartClick() {
			_sceneController?.RestartCurrentScene();
		}
	}
}