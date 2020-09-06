using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.Events;
using Grigorov.Controllers;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.UI {
    public class GameWindow : BaseWindow {
        [Space]
        [SerializeField] Button _cheatButton = null;
        [SerializeField] Button _pauseButton = null;
        
        [Space]
        [SerializeField] Text _foodCountText = null;

        void Awake() {
            _pauseButton.onClick.AddListener(OnClickPause);
            EventManager.Subscribe<FoodCalculateEvent>(this, OnFoodCalculate);
        }

        void Start() {
            var fcc = Controller.Get<BonusesController>();
            _foodCountText.text = GetFoodCountText(fcc.CurrentFoodCount, fcc.TargetFoodCount);
        }

        void OnDestroy() {
            EventManager.Unsubscribe<FoodCalculateEvent>(OnFoodCalculate);
        }

        string GetFoodCountText(int currentCount, int targetCount) {
            return $"{currentCount}/{targetCount}";
        }

        void OnClickPause() {
            Windows.ShowWindow<PauseWindow>();
            Time.timeScale = 0;
        }

        void OnFoodCalculate(FoodCalculateEvent e) {
            _foodCountText.text = GetFoodCountText(e.CurCount, e.TargetCount);
        }
    }
}