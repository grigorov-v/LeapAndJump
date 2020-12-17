using Grigorov.Save;
using Grigorov.Unity.Events;
using Grigorov.Unity.Controllers;

using Grigorov.LeapAndJump.Events;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Controllers
{
	[Controller] 
	public sealed class FoodsController : IInit, IReset
	{
		SaveableField<int> _totalFoodCount = new SaveableField<int>("FoodCount", true, 0);
		FoodsContainer              _foods          = null;

		public int CurrentFoodCount { get; private set; } = 0;
		public int TargetFoodCount  { get; private set; } = 0;
		public int SpawnCountFoods  { get; private set; } = 0;

		public int TotalFoodCount => _totalFoodCount.Value;

		LevelConfigController LevelConfigController => ControllersBox.Get<LevelConfigController>();
		LevelController       LevelController       => ControllersBox.Get<LevelController>();
		ScenesController      ScenesController      => ControllersBox.Get<ScenesController>();

		public void OnInit()
		{
			if (!ScenesController.IsActiveWorldScene)
			{
				return;
			}
			
			_totalFoodCount.Load();
			_foods = LevelConfigController.Config.Foods;

			CurrentFoodCount = 0;
			SpawnCountFoods  = 0;
			TargetFoodCount  = LevelConfigController.Config.GetFoodsCount(LevelController.CurrentLevel.Level);

			EventManager.Subscribe<SpawnLevelElementEvent>(this, OnSpawnLevelElement);
			EventManager.Subscribe<FoodCollectEvent>(this, OnFoodCollect);

			UnityEngine.Debug.Log(typeof(FoodsController).ToString());
		}

		public void OnReset()
		{
			_totalFoodCount.Save();
			EventManager.Unsubscribe<SpawnLevelElementEvent>(OnSpawnLevelElement);
			EventManager.Unsubscribe<FoodCollectEvent>(OnFoodCollect);
		}

		void OnSpawnLevelElement(SpawnLevelElementEvent e)
		{
			var element = e.LevelElement;
			SpawnCountFoods += element.SpawnFoods(_foods);
			EventManager.Fire(new FoodsController_CreateFoodEvent(SpawnCountFoods, TargetFoodCount));
		}

		void OnFoodCollect(FoodCollectEvent e)
		{
			_totalFoodCount.Value++;
			CurrentFoodCount++;
			EventManager.Fire(new FoodsController_FoodCalculateEvent(CurrentFoodCount, TargetFoodCount, TotalFoodCount));
		}
	}
}