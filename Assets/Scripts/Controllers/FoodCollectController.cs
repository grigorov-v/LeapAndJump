using Grigorov.Controllers;
using Grigorov.Save;
using Grigorov.Events;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public struct FoodCalculateEvent {
        public int CurCount;
        public int TargetCount;
        public int TotalCount;

        public FoodCalculateEvent(int curCount, int targetCount, int totalCount) {
            CurCount = curCount;
            TargetCount = targetCount;
            TotalCount = totalCount;
        }
    }

    public class FoodCollectController : IInit, IReinit, IAwake {
        SaveableField<int> _totalFoodCount = new SaveableField<int>("FoodCount", true, 0);

        public int CurrentFoodCount { get; private set; }
        public int TargetFoodCount  { get; private set; }
        public int TotalFoodCount {
            get => _totalFoodCount.Value;
        }

        void IInit.OnInit() {
            _totalFoodCount.Load();
            EventManager.Subscribe<FoodCollectEvent>(this, OnFoodCollect);
        }

        void IAwake.OnAwake() {
            CurrentFoodCount = 0;
            var bc = ControllersRegister.FindController<BalanceController>();
            var lc = ControllersRegister.FindController<LevelController>(); 
            TargetFoodCount = bc.GetFoodsCount(lc.CurrentLevel);
        }

        void IReinit.OnReinit() {
            _totalFoodCount.Save();
            EventManager.Unsubscribe<FoodCollectEvent>(OnFoodCollect);
        }

        void OnFoodCollect(FoodCollectEvent e) {
            _totalFoodCount.Value ++;
            CurrentFoodCount ++;
            EventManager.Fire(new FoodCalculateEvent(CurrentFoodCount, TargetFoodCount, TotalFoodCount));
        }
    }
}