using Grigorov.Controllers;
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

    public class FoodCollectController : IAwake, IDestroy {
        SaveableField<int> _foodCount = new SaveableField<int>().SetKey("FoodCount");

        public int FoodCount {
            get => _foodCount.Value;
        }

        public void OnAwake() {
            _foodCount.Load(0);
            EventManager.Subscribe<FoodCollectEvent>(this, OnFoodCollect);
        }

        public void OnDestroy() {
            _foodCount.Save();
            EventManager.Unsubscribe<FoodCollectEvent>(OnFoodCollect);
        }

        void OnFoodCollect(FoodCollectEvent e) {
            _foodCount.Value ++;
            _foodCount.Save();
            EventManager.Fire(new FoodCalculateEvent(_foodCount.Value));
        }
    }
}