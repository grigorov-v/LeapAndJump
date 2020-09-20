using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.Events;
using Grigorov.Controllers;
using Grigorov.LeapAndJump.Controllers;
using Grigorov.LeapAndJump.Events;

namespace Grigorov.LeapAndJump.UI {
    public class GameWindow : BaseWindow {
        [Space]
        [SerializeField] Button _pauseButton = null;
        
        [Space]
        [SerializeField] Text _foodCountText = null;

        void Awake() {
            _pauseButton.onClick.AddListener(OnClickPause);
            EventManager.Subscribe<FoodsController_FoodCalculateEvent>(this, OnFoodCalculate);
        }

        void Start() {
            var fcc = Controller.Get<FoodsController>();
            _foodCountText.text = GetFoodCountText(fcc.CurrentFoodCount, fcc.TargetFoodCount);
        }

        void OnDestroy() {
            EventManager.Unsubscribe<FoodsController_FoodCalculateEvent>(OnFoodCalculate);
        }

        string GetFoodCountText(int currentCount, int targetCount) {
            return $"{currentCount}/{targetCount}";
        }

        void OnClickPause() {
            Windows.Get<PauseWindow>().Show();
        }

        void OnFoodCalculate(FoodsController_FoodCalculateEvent e) {
            _foodCountText.text = GetFoodCountText(e.CurCount, e.TargetCount);
        }
    }
}