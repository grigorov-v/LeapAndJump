using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;

namespace Grigorov.LeapAndJump.UI {
    public class GameWindow : BaseWindow {
        [Space]
        [SerializeField] Button _cheatButton = null;

        void Awake() {
            _cheatButton.onClick.AddListener(OnClickDebug);
        }

        void OnClickDebug() {
            Windows.TakeWindow<DebugWindow>();
        }
    }
}