using Grigorov.LeapAndJump.Controllers;
using Grigorov.UI;
using Grigorov.Unity.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Grigorov.LeapAndJump.UI {
	public class WinWindow : BaseWindow {
		[SerializeField] Button _btnNextLevel;

		protected override bool PauseEnabled => true;

		void Awake() {
			_btnNextLevel.onClick.AddListener(OnClickNextLevel);
		}

		void OnClickNextLevel() {
			Hide();
			ControllersBox.Get<LevelController>().CompleteLevel();
		}
	}
}