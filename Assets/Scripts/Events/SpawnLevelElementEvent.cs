using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Events {
	public struct SpawnLevelElementEvent {
		public LevelElement LevelElement { get; }

		public SpawnLevelElementEvent(LevelElement levelElement) {
			LevelElement = levelElement;
		}
	}
}