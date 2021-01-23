namespace Grigorov.LeapAndJump.Events {
	public readonly struct FoodsController_FoodCalculateEvent {
		public int CurCount { get; }
		public int TargetCount { get; }
		public int TotalCount { get; }

		public FoodsController_FoodCalculateEvent(int curCount, int targetCount, int totalCount) {
			CurCount = curCount;
			TargetCount = targetCount;
			TotalCount = totalCount;
		}
	}
}