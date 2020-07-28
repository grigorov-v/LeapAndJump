using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.Events;
using Grigorov.Controller;
using Grigorov.LeapAndJump.Controllers;

namespace Grigorov.LeapAndJump.UI {
    public class GameWindow : BaseWindow {
        [Space]
        [SerializeField] Button _cheatButton = null;
        [SerializeField] Button _pauseButton = null;
        
        [Space]
        [SerializeField] Text _foodCountText = null;

        void Awake() {
            _cheatButton.onClick.AddListener(OnClickDebug);
            _pauseButton.onClick.AddListener(OnClickPause);
            EventManager.Subscribe<FoodCalculateEvent>(this, OnFoodCalculate);
        }

        void Start() {
            _foodCountText.text = ControllersRegister.FindController<FoodCollectController>()?.FoodCount.ToString();
        }

        void OnDestroy() {
            EventManager.Unsubscribe<FoodCalculateEvent>(OnFoodCalculate);
        }

        void OnClickDebug() {
            Windows.TakeWindow<DebugWindow>();
        }

        void OnClickPause() {
            Windows.ShowWindow<PauseWindow>();
            Time.timeScale = 0;
        }

        void OnFoodCalculate(FoodCalculateEvent e) {
            _foodCountText.text = e.Count.ToString();
        }
    }
}