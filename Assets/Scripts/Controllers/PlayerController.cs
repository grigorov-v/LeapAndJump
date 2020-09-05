using UnityEngine;

using Grigorov.Events;
using Grigorov.Controllers;
using Grigorov.Helpers;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public class PlayerController : Controller, IInit, IDeinit, IUpdate, IFixedUpdate {
        Player _player = null;

        void IInit.OnInit() {
            EventManager.Subscribe<TapZone_PointerDown>(this, OnPointerDown);
        }

        void IDeinit.OnDeinit() {
            EventManager.Unsubscribe<TapZone_PointerDown>(OnPointerDown);
        }

        void IUpdate.OnUpdate() {
            if ( Input.GetKeyDown(KeyCode.Space) ) {
                Player.AllObjects.ForEach(player => player.JumpInput());
            }
        }

        void IFixedUpdate.OnFixedUpdate() {
            Player.AllObjects.ForEach(player => player.OnUpdate());
        }

        void OnPointerDown(TapZone_PointerDown e) {
            Player.AllObjects.ForEach(player => player.JumpInput());
        }
    }
}