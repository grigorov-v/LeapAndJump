using Grigorov.LeapAndJump.Level;
using Grigorov.Unity.Controllers;

namespace Grigorov.LeapAndJump.Controllers {
	public sealed class LevelConfigController : IController {
		public LevelConfig Config { get; private set; }

		public bool IsActive => ControllersBox.Get<ScenesController>()?.IsActiveWorldScene ?? false;

		ScenesController ScenesController => ControllersBox.Get<ScenesController>();

		public void OnInit() {
			Config = LevelConfig.Load(ScenesController.CurrentSceneName);
		}

		public void OnReset() {
			Config = null;
		}
	}
}