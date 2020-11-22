using UnityEngine;
using System.Collections;

using Grigorov.UI;
using Grigorov.Save;
using Grigorov.Unity.Events;
using Grigorov.Controllers;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.Events;

namespace Grigorov.LeapAndJump.Controllers
{
	[Controller]
	public sealed class LevelController : IAwake, IDestroy
	{
		SaveableField<LevelId> _currentLevel = new SaveableField<LevelId>("CurrentLevel", defaultValue: new LevelId("World_0", 0));

		bool _isCanShowEndLevelWindow = false;

		public LevelId CurrentLevel => _currentLevel.Value;

		public bool IsLevelFinish { get; private set; } = false;

		ScenesController ScenesController => Controller.Get<ScenesController>();

		public void OnAwake()
		{
			IsLevelFinish = false;
			_isCanShowEndLevelWindow = false;
			_currentLevel.Load();

			EventManager.Subscribe<FoodsController_CreateFoodEvent>(this, OnCreateFood);
			EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
		}

		public void OnDestroy()
		{
			EventManager.Unsubscribe<FoodsController_CreateFoodEvent>(OnCreateFood);
			EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
		}

		public void CompleteLevel()
		{
			_currentLevel.Value = new LevelId(CurrentLevel.World, CurrentLevel.Level + 1);
			ScenesController.RestartCurrentScene();
		}

		IEnumerator ShowWinWindow()
		{
			yield return new WaitForSeconds(1.5f);
			if (_isCanShowEndLevelWindow)
			{
				Windows.Get<WinWindow>().Show();
				_isCanShowEndLevelWindow = false;
			}
		}

		void OnCreateFood(FoodsController_CreateFoodEvent e)
		{
			if (!IsLevelFinish)
			{
				IsLevelFinish = e.SpawnCount >= e.TargetCount;
			}
		}

		void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e)
		{
			var levelBlock = e.LevelBlock;
			if (!levelBlock)
			{
				return;
			}

			_isCanShowEndLevelWindow = IsLevelFinish && levelBlock.IsWinBlock;

			var cp = ControllersProcessor.Instance;
			cp.StopCoroutine(ShowWinWindow());
			cp.StartCoroutine(ShowWinWindow());
		}
	}
}