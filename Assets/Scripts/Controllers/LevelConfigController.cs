using Grigorov.LeapAndJump.Level;
using Grigorov.Unity.Controllers;
using UnityEngine;

namespace Grigorov.LeapAndJump.Controllers {
	public sealed class LevelConfigController : IController {
		public LevelConfig Config { get; private set; }

		ScenesController ScenesController => ControllersBox.Get<ScenesController>();

		public void OnInit() {
			Config = LevelConfig.Load(ScenesController.CurrentSceneName);
			Debug.Log(typeof(LevelConfigController).ToString());
		}

		public void OnReset() {
			Config = null;
		}
	}
}