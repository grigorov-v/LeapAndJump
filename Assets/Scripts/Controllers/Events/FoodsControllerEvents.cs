namespace Grigorov.LeapAndJump.Controllers.Events {
    public struct FoodsController_CreateFoodEvent {
        public int SpawnCount  { get; private set; }
        public int TargetCount { get; private set; }

        public FoodsController_CreateFoodEvent(int spawnCount, int targetCount) {
            SpawnCount  = spawnCount;
            TargetCount = targetCount;
        }
    }

    public struct FoodsController_FoodCalculateEvent {
        public int CurCount    { get; private set; }
        public int TargetCount { get; private set; }
        public int TotalCount  { get; private set; }

        public FoodsController_FoodCalculateEvent(int curCount, int targetCount, int totalCount) {
            CurCount    = curCount;
            TargetCount = targetCount;
            TotalCount  = totalCount;
        }
    }
}