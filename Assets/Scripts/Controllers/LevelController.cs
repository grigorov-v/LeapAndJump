using Grigorov.Controllers;
using Grigorov.Save;
using Grigorov.Events;

using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public class LevelController : Controller {
        SaveableField<LevelId> _currentLevel      = new SaveableField<LevelId>("CurrentLevel", defaultValue: new LevelId("World_1", 0));
        FoodsController        _bonusesController = null;

        public bool IsLevelFinish { get; private set; }
        
        public LevelId CurrentLevel {
            get => _currentLevel.Value;
        }

        public override void OnInit() {
            _currentLevel.Load();
            _bonusesController = Controller.Get<FoodsController>();
            
            EventManager.Subscribe<CreateFoodEvent>(this, OnCreateFood);
            EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
        }

        public override void OnAwake() {
            IsLevelFinish = false; 
        }

        public override void OnDeinit() {
            EventManager.Unsubscribe<CreateFoodEvent>(OnCreateFood);
            EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
        }

        void OnWinLevel() {
            UnityEngine.Debug.Log("Win level");
        }

        void OnCreateFood(CreateFoodEvent e) {
            if ( !IsLevelFinish ) {
                IsLevelFinish = e.SpawnCount >= e.TargetCount;
            }
        }

        void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e) {
            if ( IsLevelFinish ) {
                OnWinLevel();
            }
        }
    }
}