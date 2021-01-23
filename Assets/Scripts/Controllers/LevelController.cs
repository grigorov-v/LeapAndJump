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
		SaveableField<LevelId> _currentLevel;
		bool                   _isCanShowEndLevelWindow;

		public bool IsLevelFinish { get; private set; }

		public LevelId CurrentLevel => _currentLevel.Value;
		
		public bool IsActive => true;

		ScenesController ScenesController => ControllersBox.Get<ScenesController>();

		public void OnInit() {
			IsLevelFinish = false;
			_isCanShowEndLevelWindow = false;
			
			_currentLevel = new SaveableField<LevelId>("CurrentLevel", defaultValue: new LevelId("World_0", 0));
			_currentLevel.Load();

			EventManager.Subscribe<FoodsController_CreateFoodEvent>(this, OnCreateFood);
			EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
		}

		public void OnReset() {
			EventManager.Unsubscribe<FoodsController_CreateFoodEvent>(OnCreateFood);
			EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);

			_currentLevel = null;
		}

		public void CompleteLevel() {
			_currentLevel.Value = new LevelId(CurrentLevel.World, CurrentLevel.Level + 1);
			ScenesController.RestartCurrentScene();
		}

		IEnumerator ShowWinWindow() {
			yield return new WaitForSeconds(1.5f);
			if ( !_isCanShowEndLevelWindow ) {
				yield break;
			}

			Windows.Get<WinWindow>().Show();
			_isCanShowEndLevelWindow = false;
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