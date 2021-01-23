namespace Grigorov.LeapAndJump.Events {
	public readonly struct ScenesController_LoadedSceneEvent {
		public string Scene { get; }

		public ScenesController_LoadedSceneEvent(string scene) {
			Scene = scene;
		}
	}
}