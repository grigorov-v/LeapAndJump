namespace Grigorov.LeapAndJump.Level
{
	public struct LevelId
	{
		public string World { get; private set; }
		public int    Level { get; private set; }

		public LevelId(string world, int level)
		{
			World = world;
			Level = level;
		}
	}
}