using UnityEngine;

using Grigorov.Events;
using Grigorov.Controllers;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Player;

namespace Grigorov.LeapAndJump.Controllers {
    public class PlayerController : IInit, IDeinit, IUpdate, IFixedUpdate {
        PlayerControl _player    = null;

        PlayerControl Player {
            get {
                if ( !_player ) {
                    _player = GameObject.FindObjectOfType<PlayerControl>();
                }
                return _player;
            }
        }

        void IInit.OnInit() {
            EventManager.Subscribe<TapZone_PointerDown>(this, OnPointerDown);
        }

        void IDeinit.OnDeinit() {
            EventManager.Unsubscribe<TapZone_PointerDown>(OnPointerDown);
        }

        void IUpdate.OnUpdate() {
            if ( Input.GetKeyDown(KeyCode.Space) ) {
                Player.JumpInput();
            }
        }

        void IFixedUpdate.OnFixedUpdate() {
            Player.OnUpdate();
        }

        void OnPointerDown(TapZone_PointerDown e) {
            Player.JumpInput();
        }
    }
}