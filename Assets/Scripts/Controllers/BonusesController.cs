using Grigorov.Controllers;
using Grigorov.Save;
using Grigorov.Events;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Controllers {
    public struct CreateFoodEvent {
        public int Count { get; private set; }

        public CreateFoodEvent(int count) {
            Count = count;
        }
    }

    public struct FoodCalculateEvent {
        public int CurCount;
        public int TargetCount;
        public int TotalCount;

        public FoodCalculateEvent(int curCount, int targetCount, int totalCount) {
            CurCount = curCount;
            TargetCount = targetCount;
            TotalCount = totalCount;
        }
    }

    public class BonusesController : Controller {
        SaveableField<int> _totalFoodCount = new SaveableField<int>("FoodCount", true, 0);
        Foods              _foods          = null;

        public int CurrentFoodCount { get; private set; }
        public int TargetFoodCount  { get; private set; }
        public int TotalFoodCount {
            get => _totalFoodCount.Value;
        }

        public override void OnInit() {
            _totalFoodCount.Load();
            EventManager.Subscribe<SpawnLevelElementEvent>(this, OnSpawnLevelElement);
            EventManager.Subscribe<FoodCollectEvent>(this, OnFoodCollect);
        }

        public override void OnAwake() {
            CurrentFoodCount = 0;
            var bc = Controller.FindController<BalanceController>();
            var lc = Controller.FindController<LevelController>(); 
            TargetFoodCount = bc.GetFoodsCount(lc.CurrentLevel);

            var level = Controller.FindController<LevelController>()?.CurrentLevel;
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
            var foodsCount = element.SpawnFoods(_foods);
            EventManager.Fire(new CreateFoodEvent(foodsCount));
        }

        void OnFoodCollect(FoodCollectEvent e) {
            _totalFoodCount.Value ++;
            CurrentFoodCount ++;
            EventManager.Fire(new FoodCalculateEvent(CurrentFoodCount, TargetFoodCount, TotalFoodCount));
        }
    }
}