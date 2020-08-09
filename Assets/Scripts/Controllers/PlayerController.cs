using UnityEngine;

using Grigorov.Events;
using Grigorov.Controllers;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public class PlayerController : IInit, IDeinit, IUpdate, IFixedUpdate {
        Player _player = null;

        public void SetPlayer(Player player) {
            _player = player;
        }

        void IInit.OnInit() {
            EventManager.Subscribe<TapZone_PointerDown>(this, OnPointerDown);
        }

        void IDeinit.OnDeinit() {
            EventManager.Unsubscribe<TapZone_PointerDown>(OnPointerDown);
        }

        void IUpdate.OnUpdate() {
            if ( !_player ) {
                return;
            }
            if ( Input.GetKeyDown(KeyCode.Space) ) {
                _player.JumpInput();
            }
        }

        void IFixedUpdate.OnFixedUpdate() {
            if ( !_player ) {
                return;
            }
            _player.OnUpdate();
        }

        void OnPointerDown(TapZone_PointerDown e) {
            if ( !_player ) {
                return;
            }
            _player.JumpInput();
        }
    }
}