using Grigorov.Controller;
using Grigorov.Save;
using Grigorov.Events;

using Grigorov.LeapAndJump.Level.Gameplay;

namespace Grigorov.LeapAndJump.Controllers {
    public struct FoodCalculateEvent {
        public int Count;

        public FoodCalculateEvent(int count) {
            Count = count;
        }
    }

    public class FoodCollectController : IController {
        SaveableField<int> _foodCount = new SaveableField<int>().SetKey("FoodCount");

        public int FoodCount {
            get => _foodCount.Value;
        }

        public void Init() {
            _foodCount.Load(0);
            EventManager.Subscribe<FoodCollectEvent>(this, OnFoodCollect);
        }

        public void Reinit() {
            _foodCount.Save();
            EventManager.Unsubscribe<FoodCollectEvent>(OnFoodCollect);
        }

        void OnFoodCollect(FoodCollectEvent e) {
            _foodCount.Value ++;
            EventManager.Fire(new FoodCalculateEvent(_foodCount.Value));
        }
    }
}