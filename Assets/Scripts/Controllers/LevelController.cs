using System.Collections;
using Grigorov.LeapAndJump.Events;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.UI;
using Grigorov.Save;
using Grigorov.UI;
using Grigorov.Unity.Controllers;
using Grigorov.Unity.Events;
using UnityEngine;

namespace Grigorov.LeapAndJump.Controllers {
	public sealed class LevelController : IController {
		readonly SaveableField<LevelId> _currentLevel =
			new SaveableField<LevelId>("CurrentLevel", defaultValue: new LevelId("World_0", 0));

		bool _isCanShowEndLevelWindow;

		public bool IsLevelFinish { get; private set; }

		public LevelId CurrentLevel => _currentLevel.Value;

		ScenesController ScenesController => ControllersBox.Get<ScenesController>();

		public void OnInit() {
			IsLevelFinish = false;
			_isCanShowEndLevelWindow = false;
			_currentLevel.Load();

			EventManager.Subscribe<FoodsController_CreateFoodEvent>(this, OnCreateFood);
			EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);

			Debug.Log(typeof(LevelController).ToString());
		}

		public void OnReset() {
			EventManager.Unsubscribe<FoodsController_CreateFoodEvent>(OnCreateFood);
			EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
		}

		public void CompleteLevel() {
			_currentLevel.Value = new LevelId(CurrentLevel.World, CurrentLevel.Level + 1);
			ScenesController.RestartCurrentScene();
		}

		IEnumerator ShowWinWindow() {
			yield return new WaitForSeconds(1.5f);
			if ( _isCanShowEndLevelWindow ) {
				Windows.Get<WinWindow>().Show();
				_isCanShowEndLevelWindow = false;
			}
		}

		void OnCreateFood(FoodsController_CreateFoodEvent e) {
			if ( !IsLevelFinish ) {
				IsLevelFinish = e.SpawnCount >= e.TargetCount;
			}
		}

		void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e) {
			var levelBlock = e.LevelBlock;
			if ( !levelBlock ) {
				return;
			}

			_isCanShowEndLevelWindow = IsLevelFinish && levelBlock.IsWinBlock;

			var cp = ControllersProcessor.Instance;
			cp.StopCoroutine(ShowWinWindow());
			cp.StartCoroutine(ShowWinWindow());
		}
	}
}