using Grigorov.Unity.Controllers;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers
{
	public sealed class LevelConfigController : IController
	{
		public LevelConfig Config { get; private set; } = null;

		ScenesController ScenesController => ControllersBox.Get<ScenesController>();

		public void OnInit()
		{
			Config = LevelConfig.Load(ScenesController.CurrentSceneName);
			UnityEngine.Debug.Log(typeof(LevelConfigController).ToString());
		}

		public void OnReset()
		{
			Config = null;
		}
	}
}