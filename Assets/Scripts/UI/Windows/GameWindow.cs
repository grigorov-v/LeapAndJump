using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;
using Grigorov.Unity.Events;
using Grigorov.Unity.Controllers;
using Grigorov.LeapAndJump.Controllers;
using Grigorov.LeapAndJump.Events;

namespace Grigorov.LeapAndJump.UI
{
	public class GameWindow : BaseWindow
	{
		[Space]
		[SerializeField] Button _pauseButton   = null;
		
		[Space]
		[SerializeField] Text   _foodCountText = null;
		[SerializeField] Image  _foodImage     = null;

		void Awake()
		{
			_pauseButton.onClick.AddListener(OnClickPause);
			EventManager.Subscribe<FoodsController_FoodCalculateEvent>(this, OnFoodCalculate);
		}

		void Start()
		{
			var fcc = ControllersBox.Get<FoodsController>();
			_foodCountText.text = fcc.CurrentFoodCount.ToString();
		}

		void OnDestroy()
		{
			EventManager.Unsubscribe<FoodsController_FoodCalculateEvent>(OnFoodCalculate);
		}

		void OnClickPause()
		{
			Windows.Get<PauseWindow>().Show();
		}

		void OnFoodCalculate(FoodsController_FoodCalculateEvent e)
		{
			_foodCountText.text = e.CurCount.ToString();
		}
	}
}