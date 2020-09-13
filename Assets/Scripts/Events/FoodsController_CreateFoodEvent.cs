namespace Grigorov.LeapAndJump.Events {
    public struct FoodsController_CreateFoodEvent {
        public int SpawnCount  { get; private set; }
        public int TargetCount { get; private set; }

        public FoodsController_CreateFoodEvent(int spawnCount, int targetCount) {
            SpawnCount  = spawnCount;
            TargetCount = targetCount;
        }
    }
}