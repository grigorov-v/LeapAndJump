using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.Controllers;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.UI
{
	public class PauseWindow : BaseWindow
	{
		[Space]
		[SerializeField] Button _closeButton    = null;
		[SerializeField] Button _mainMenuButton = null;
		[SerializeField] Button _restartButton  = null;

		ScenesController _sceneController = null;

		protected override bool PauseEnabled => true;

		void Awake()
		{
			_closeButton.onClick.AddListener(OnCloseClick);
			_mainMenuButton.onClick.AddListener(OnMainMenuClick);
			_restartButton.onClick.AddListener(OnRestartClick);
			_sceneController = Controller.Get<ScenesController>();
		}

		void OnCloseClick()
		{
			Hide();
		}

		void OnMainMenuClick()
		{
			_sceneController?.OpenMainMenu();
		}

		void OnRestartClick()
		{
			_sceneController?.RestartCurrentScene();
		}
	}
}