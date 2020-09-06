using Grigorov.Controllers;
using Grigorov.Save;
using Grigorov.Events;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Controllers {
    public struct CreateFoodEvent {
        public int SpawnCount  { get; private set; }
        public int TargetCount { get; private set; }

        public CreateFoodEvent(int spawnCount, int targetCount) {
            SpawnCount  = spawnCount;
            TargetCount = targetCount;
        }
    }

    public struct FoodCalculateEvent {
        public int CurCount    { get; private set; }
        public int TargetCount { get; private set; }
        public int TotalCount  { get; private set; }

        public FoodCalculateEvent(int curCount, int targetCount, int totalCount) {
            CurCount    = curCount;
            TargetCount = targetCount;
            TotalCount  = totalCount;
        }
    }

    public class FoodsController : Controller {
        SaveableField<int> _totalFoodCount = new SaveableField<int>("FoodCount", true, 0);
        Foods              _foods          = null;

        public int CurrentFoodCount { get; private set; }
        public int TargetFoodCount  { get; private set; }
        public int TotalFoodCount   { get => _totalFoodCount.Value; }

        public int SpawnCountFoods  { get; private set; }

        public override void OnInit() {
            _totalFoodCount.Load();
            EventManager.Subscribe<SpawnLevelElementEvent>(this, OnSpawnLevelElement);
            EventManager.Subscribe<FoodCollectEvent>(this, OnFoodCollect);
        }

        public override void OnAwake() {
            CurrentFoodCount = 0;
            SpawnCountFoods = 0;
            var bc = Controller.Get<BalanceController>();
            var lc = Controller.Get<LevelController>(); 
            TargetFoodCount = bc.GetFoodsCount(lc.CurrentLevel);

            var level = Controller.Get<LevelController>()?.CurrentLevel;
            _foods = Foods.Load("Foods", $"{level.Value.World}_Foods");
        }

        public override void OnDestroy() {
            _foods = null;
        }

        public override void OnDeinit() {
            _totalFoodCount.Save();
            EventManager.Unsubscribe<SpawnLevelElementEvent>(OnSpawnLevelElement);
            EventManager.Unsubscribe<FoodCollectEvent>(OnFoodCollect);
        }

        void OnSpawnLevelElement(SpawnLevelElementEvent e) {
            var element = e.LevelElement;
            SpawnCountFoods += element.SpawnFoods(_foods);
            EventManager.Fire(new CreateFoodEvent(SpawnCountFoods, TargetFoodCount));
        }

        void OnFoodCollect(FoodCollectEvent e) {
            _totalFoodCount.Value ++;
            CurrentFoodCount ++;
            EventManager.Fire(new FoodCalculateEvent(CurrentFoodCount, TargetFoodCount, TotalFoodCount));
        }
    }
}