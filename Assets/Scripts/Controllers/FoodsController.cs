using Grigorov.LeapAndJump.Events;
using Grigorov.LeapAndJump.ResourcesContainers;
using Grigorov.Save;
using Grigorov.Unity.Controllers;
using Grigorov.Unity.Events;
using UnityEngine;

namespace Grigorov.LeapAndJump.Controllers {
	public sealed class FoodsController : IController {
		FoodsContainer _foods;
		readonly SaveableField<int> _totalFoodCount = new SaveableField<int>("FoodCount", true);

		public int CurrentFoodCount { get; private set; }
		public int TargetFoodCount { get; private set; }
		public int SpawnCountFoods { get; private set; }

		public int TotalFoodCount => _totalFoodCount.Value;

		LevelConfigController LevelConfigController => ControllersBox.Get<LevelConfigController>();
		LevelController LevelController => ControllersBox.Get<LevelController>();
		ScenesController ScenesController => ControllersBox.Get<ScenesController>();

		public void OnInit() {
			if ( !ScenesController.IsActiveWorldScene ) {
				return;
			}

			_totalFoodCount.Load();
			_foods = LevelConfigController.Config.Foods;

			CurrentFoodCount = 0;
			SpawnCountFoods = 0;
			TargetFoodCount = LevelConfigController.Config.GetFoodsCount(LevelController.CurrentLevel.Level);

			EventManager.Subscribe<SpawnLevelElementEvent>(this, OnSpawnLevelElement);
			EventManager.Subscribe<FoodCollectEvent>(this, OnFoodCollect);

			Debug.Log(typeof(FoodsController).ToString());
		}

		public void OnReset() {
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
			_totalFoodCount.Value++;
			CurrentFoodCount++;
			EventManager.Fire(
				new FoodsController_FoodCalculateEvent(CurrentFoodCount, TargetFoodCount, TotalFoodCount));
		}
	}
}