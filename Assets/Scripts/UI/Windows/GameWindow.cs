using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.EventsHelper;
using Grigorov.LeapAndJump.Level.Gameplay;

namespace Grigorov.LeapAndJump.UI {
    public class GameWindow : BaseWindow {
        [Space]
        [SerializeField] Button _cheatButton = null;
        [SerializeField] Button _pauseButton = null;
        
        [Space]
        [SerializeField] Text _foodCountText = null;

        int _tempFoodCount = 0;

        void Awake() {
            _cheatButton.onClick.AddListener(OnClickDebug);
            _pauseButton.onClick.AddListener(OnClickPause);
            _foodCountText.text = _tempFoodCount.ToString();
            
            EventManager.Subscribe<FoodCollectEvent>(this, OnFoodCollect);
        }

        void OnDestroy() {
            EventManager.Unsubscribe<FoodCollectEvent>(OnFoodCollect);
        }

        void OnClickDebug() {
            Windows.TakeWindow<DebugWindow>();
        }

        void OnClickPause() {
            Windows.ShowWindow<PauseWindow>();
            Time.timeScale = 0;
        }

        void OnFoodCollect(FoodCollectEvent e) {
            _tempFoodCount ++;
            _foodCountText.text = _tempFoodCount.ToString();
        }
    }
}