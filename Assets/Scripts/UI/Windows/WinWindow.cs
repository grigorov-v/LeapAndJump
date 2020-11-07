using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.Controllers;

using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.UI
{
	public class WinWindow : BaseWindow
	{
		[SerializeField] Button _btnNextLevel = null;

		protected override bool PauseEnabled => true;

		void Awake()
		{
			_btnNextLevel.onClick.AddListener(OnClickNextLevel);
		}

		void OnClickNextLevel()
		{
			Hide();
			Controller.Get<LevelController>().CompleteLevel();
		}
	}
}