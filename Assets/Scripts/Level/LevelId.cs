namespace Grigorov.LeapAndJump.Level {
	public struct LevelId {
		public string World { get; }
		public int Level { get; }

		public LevelId(string world, int level) {
			World = world;
			Level = level;
		}
	}
}