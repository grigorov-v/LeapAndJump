﻿namespace Grigorov.LeapAndJump.Events {
	public readonly struct FoodsController_CreateFoodEvent {
		public int SpawnCount { get; }
		public int TargetCount { get; }

		public FoodsController_CreateFoodEvent(int spawnCount, int targetCount) {
			SpawnCount = spawnCount;
			TargetCount = targetCount;
		}
	}
}