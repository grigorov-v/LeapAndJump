﻿using UnityEngine;

using Grigorov.Events;
using Grigorov.Controllers;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public class PlayerController : Controller {
        Player _player = null;

        public override void OnInit() {
            EventManager.Subscribe<TapZone_PointerDown>(this, OnPointerDown);
        }

        public override void OnDeinit() {
            EventManager.Unsubscribe<TapZone_PointerDown>(OnPointerDown);
        }

        public override void OnUpdate() {
            if ( Input.GetKeyDown(KeyCode.Space) ) {
                Player.AllObjects.ForEach(player => player.JumpInput());
            }
        }

        public override void OnFixedUpdate() {
            Player.AllObjects.ForEach(player => player.OnUpdate());
        }

        void OnPointerDown(TapZone_PointerDown e) {
            Player.AllObjects.ForEach(player => player.JumpInput());
        }
    }
}