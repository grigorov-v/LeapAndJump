using Grigorov.Controllers;
using Grigorov.Save;
using Grigorov.Events;

using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public class LevelController : Controller {
        SaveableField<LevelId> _currentLevel = new SaveableField<LevelId>("CurrentLevel", defaultValue: new LevelId("World_1", 0));

        public bool IsLevelFinish { get; private set; }
        
        public LevelId CurrentLevel {
            get => _currentLevel.Value;
        }

        public override void OnInit() {
            _currentLevel.Load();
            EventManager.Subscribe<FoodCalculateEvent>(this, OnFoodCalculate);
            EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
        }

        public override void OnAwake() {
            IsLevelFinish = false; 
        }

        public override void OnDeinit() {
            EventManager.Unsubscribe<FoodCalculateEvent>(OnFoodCalculate);
            EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
        }

        void OnWinLevel() {
            UnityEngine.Debug.Log("Win level");
        }

        void OnFoodCalculate(FoodCalculateEvent e) {
            if ( !IsLevelFinish ) {
                IsLevelFinish = e.CurCount >= e.TargetCount;
            }
        }

        void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e) {
            if ( IsLevelFinish ) {
                OnWinLevel();
            }
        }
    }
}