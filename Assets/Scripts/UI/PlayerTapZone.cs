using UnityEngine;
using UnityEngine.EventSystems;

using Grigorov.LeapAndJump.Player;

namespace Grigorov.LeapAndJump.UI {
    public class PlayerTapZone : MonoBehaviour, IPointerDownHandler {
        PlayerControl _playerControl = null;

        PlayerControl PlayerControl {
            get {
                if ( !_playerControl ) {
                    _playerControl = FindObjectOfType<PlayerControl>();
                }
                return _playerControl;
            }
        }

        public void OnPointerDown(PointerEventData pointerEventData) {
            PlayerControl.JumpInput();
        }
    }
}