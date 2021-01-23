namespace Grigorov.LeapAndJump.Level {
	public readonly struct LevelId {
		public readonly string World;
		public readonly int    Level;

		public LevelId(string world, int level) {
			World = world;
			Level = level;
		}
	}
}