﻿using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.Unity.Controllers;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.UI
{
	public class MainMenuWindow : BaseWindow
	{
		[Space]
		[SerializeField] Button _startGameButton = null;

		ScenesController _sceneController = null;

		void Awake()
		{
			_startGameButton.onClick.AddListener(OnStartClick);
			_sceneController = ControllersBox.Get<ScenesController>();
		}

		void OnStartClick()
		{
			var lc = ControllersBox.Get<LevelController>();
			_sceneController?.OpenLevel(lc.CurrentLevel);
		}
	}
}