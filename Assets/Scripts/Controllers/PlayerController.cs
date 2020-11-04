using UnityEngine;

using Grigorov.Unity.Events;
using Grigorov.Controllers;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public class PlayerController : Controller {
        public override void OnInit() {
            EventManager.Subscribe<TapZone_PointerDown>(this, OnPointerDown);
        }

        public override void OnDeinit() {
            EventManager.Unsubscribe<TapZone_PointerDown>(OnPointerDown);
        }

        public override void OnUpdate() {
            if ( Input.GetKeyDown(KeyCode.Space) ) {
                Player.ForAll(player => player.JumpInput());
            }
        }

        public override void OnFixedUpdate() {
            Player.ForAll(player => player.OnUpdate());
        }

        void OnPointerDown(TapZone_PointerDown e) {
            Player.ForAll(player => player.JumpInput());
        }
    }
}