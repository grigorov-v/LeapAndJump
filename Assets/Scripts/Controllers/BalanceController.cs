using System.Collections.Generic;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public class BalanceController {
        Balance _lastLoadedBalance = null;

        public List<string> GetElementsGroups(LevelId levelId) {
            return GetBalance(levelId.World)?.GetElementsGroups(levelId.Level);
        }

        public Balance GetBalance(string world) {
            if ( (_lastLoadedBalance == null) || (_lastLoadedBalance.World != world) ) {
                _lastLoadedBalance = new Balance(world);
            }
            return _lastLoadedBalance;
        }
    }
}