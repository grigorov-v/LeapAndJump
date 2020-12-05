using Grigorov.Unity.Controllers;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers
{
	[Controller]
	public sealed class LevelConfigController : IInit
	{
		public LevelConfig Config { get; private set; }

		ScenesController ScenesController => ControllersBox.Get<ScenesController>();

		public void OnInit()
		{
			Config = LevelConfig.Load(ScenesController.CurrentSceneName);
			
			UnityEngine.Debug.Log(typeof(LevelConfigController).ToString());
		}
	}
}