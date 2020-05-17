using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Grigorov.UI;

namespace Grigorov.LeapAndJump.UI {
    public class GameWindow : BaseWindow {
        [Space]
        [SerializeField] Button _cheatButton = null;
        [SerializeField] Button _pauseButton = null;

        void Awake() {
            _cheatButton.onClick.AddListener(OnClickDebug);
            _pauseButton.onClick.AddListener(OnClickPause);
        }

        void OnClickDebug() {
            Windows.TakeWindow<DebugWindow>();
        }

        void OnClickPause() {
            Windows.ShowWindow<PauseWindow>();
            Time.timeScale = 0;
        }
    }
}