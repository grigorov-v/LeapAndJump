using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Events
{
	public struct SpawnLevelElementEvent
	{
		public LevelElement LevelElement { get; private set; }

		public SpawnLevelElementEvent(LevelElement levelElement)
		{
			LevelElement = levelElement;
		}
	}
}