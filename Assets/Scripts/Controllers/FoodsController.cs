using Grigorov.LeapAndJump.Events;
using Grigorov.LeapAndJump.ResourcesContainers;
using Grigorov.Save;
using Grigorov.Unity.Controllers;
using Grigorov.Unity.Events;

namespace Grigorov.LeapAndJump.Controllers {
	public sealed class FoodsController : IController {
		SaveableField<int> _totalFoodCount;
		FoodsContainer     _foods;

		public bool IsActive => ControllersBox.Get<ScenesController>()?.IsActiveWorldScene ?? false;

		public int CurrentFoodCount { get; private set; }

		int _targetFoodCount;
		int _spawnCountFoods;

		LevelConfigController LevelConfigController => ControllersBox.Get<LevelConfigController>();
		LevelController       LevelController       => ControllersBox.Get<LevelController>();

		public void OnInit() {
			_totalFoodCount = new SaveableField<int>("FoodCount", true);
			_totalFoodCount.Load();
			
			_foods = LevelConfigController.Config.Foods;

			CurrentFoodCount = 0;
			_spawnCountFoods = 0;
			_targetFoodCount = LevelConfigController.Config.GetFoodsCount(LevelController.CurrentLevel.Level);

			EventManager.Subscribe<SpawnLevelElementEvent>(this, OnSpawnLevelElement);
			EventManager.Subscribe<FoodCollectEvent>(this, OnFoodCollect);
		}

		public void OnReset() {
			_totalFoodCount.Save();
			EventManager.Unsubscribe<SpawnLevelElementEvent>(OnSpawnLevelElement);
			EventManager.Unsubscribe<FoodCollectEvent>(OnFoodCollect);
			
			_totalFoodCount = null;
			_foods = null;
		}

		void OnSpawnLevelElement(SpawnLevelElementEvent e) {
			_spawnCountFoods += e.LevelElement.SpawnFoods(_foods);
			EventManager.Fire(new FoodsController_CreateFoodEvent(_spawnCountFoods, _targetFoodCount));
		}

		void OnFoodCollect(FoodCollectEvent e) {
			_totalFoodCount.Value++;
			CurrentFoodCount++;
			EventManager.Fire(
				new FoodsController_FoodCalculateEvent(CurrentFoodCount, _targetFoodCount, _totalFoodCount.Value));
		}
	}
}