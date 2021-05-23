using Grigorov.LeapAndJump.Controllers;
using Grigorov.LeapAndJump.Events;
using Grigorov.UI;
using Grigorov.Unity.Controllers;
using Grigorov.Unity.Events;
using Grigorov.Unity.SerializedPropertyValidator.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Grigorov.LeapAndJump.UI {
	public class GameWindow : BaseWindow {
		[Space] 
		[ExpectNotNull][SerializeField] Button _pauseButton;
		
		[Space] 
		[ExpectNotNull][SerializeField] Text   _foodCountText;

		void Awake() {
			_pauseButton.onClick.AddListener(OnClickPause);
			EventManager.Subscribe<FoodsController_FoodCalculateEvent>(this, OnFoodCalculate);
		}

		void Start() {
			var fcc = ControllersBox.Get<FoodsController>();
			_foodCountText.text = fcc.CurrentFoodCount.ToString();
		}

		void OnDestroy() {
			EventManager.Unsubscribe<FoodsController_FoodCalculateEvent>(OnFoodCalculate);
		}

		void OnClickPause() {
			Windows.Get<PauseWindow>().Show();
		}

		void OnFoodCalculate(FoodsController_FoodCalculateEvent e) {
			_foodCountText.text = e.CurCount.ToString();
		}
	}
}