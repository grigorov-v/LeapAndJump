using Grigorov.Save;
using Grigorov.Unity.Events;
using Grigorov.Controllers;

using Grigorov.LeapAndJump.Events;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Controllers {
    public class FoodsController : Controller {
        SaveableField<int> _totalFoodCount = new SaveableField<int>("FoodCount", true, 0);
        Foods              _foods          = null;

        public int CurrentFoodCount { get; private set; } = 0;
        public int TargetFoodCount  { get; private set; } = 0;
        public int SpawnCountFoods  { get; private set; } = 0;

        public int TotalFoodCount   { get => _totalFoodCount.Value; }

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
            EventManager.Fire(new FoodsController_CreateFoodEvent(SpawnCountFoods, TargetFoodCount));
        }

        void OnFoodCollect(FoodCollectEvent e) {
            _totalFoodCount.Value ++;
            CurrentFoodCount ++;
            EventManager.Fire(new FoodsController_FoodCalculateEvent(CurrentFoodCount, TargetFoodCount, TotalFoodCount));
        }
    }
}