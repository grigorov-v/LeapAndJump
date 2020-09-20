using UnityEngine;
using System.Collections;

using Grigorov.UI;
using Grigorov.Save;
using Grigorov.Events;
using Grigorov.Controllers;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.Events;

namespace Grigorov.LeapAndJump.Controllers {
    public class LevelController : Controller {
        SaveableField<LevelId> _currentLevel      = new SaveableField<LevelId>("CurrentLevel", defaultValue: new LevelId("World_0", 0));
        FoodsController        _bonusesController = null;
        ScenesController       _scenesController  = null;
        
        bool  _isCanShowEndLevelWindow = false;

        public bool IsLevelFinish { get; private set; }
        
        public LevelId CurrentLevel {
            get => _currentLevel.Value;
        }

        public override void OnInit() {
            _currentLevel.Load();
            _bonusesController = Controller.Get<FoodsController>();
            _scenesController = Controller.Get<ScenesController>();
            _isCanShowEndLevelWindow = false;

            IsLevelFinish = false;
            
            EventManager.Subscribe<FoodsController_CreateFoodEvent>(this, OnCreateFood);
            EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
        }

        public override void OnAwake() {
            IsLevelFinish = false;
            _isCanShowEndLevelWindow = false;
        }

        public override void OnDeinit() {
            EventManager.Unsubscribe<FoodsController_CreateFoodEvent>(OnCreateFood);
            EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
        }

        public void CompleteLevel() {
            _currentLevel.Value = new LevelId(CurrentLevel.World, CurrentLevel.Level + 1);
            _scenesController.RestartCurrentScene();
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
            ControllersProcessor.Main.StartCoroutine(ShowWinWindow());
        }
    }
}