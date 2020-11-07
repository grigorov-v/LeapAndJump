
namespace Grigorov.LeapAndJump.Events
{
	public struct ScenesController_LoadedSceneEvent
	{
		public string Scene { get; private set; }

		public ScenesController_LoadedSceneEvent(string scene)
		{
			Scene = scene;
		}
	}
}