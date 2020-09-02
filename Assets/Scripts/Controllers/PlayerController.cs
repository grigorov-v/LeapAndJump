using UnityEngine;

using Grigorov.Events;
using Grigorov.Controllers;
using Grigorov.Helpers;

using Grigorov.LeapAndJump.UI;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Controllers {
    public class PlayerController : AbstractController, IInit, IDeinit, IUpdate, IFixedUpdate {
        Player _player = null;

        public Player Player {
            get => ComponentHelper.GetOrFindComponent(ref _player, () => ControlledBehaviours.Find(c => c is Player) as Player);
        }

        void IInit.OnInit() {
            EventManager.Subscribe<TapZone_PointerDown>(this, OnPointerDown);
        }

        void IDeinit.OnDeinit() {
            EventManager.Unsubscribe<TapZone_PointerDown>(OnPointerDown);
        }

        void IUpdate.OnUpdate() {
            if ( !Player ) {
                return;
            }
            if ( Input.GetKeyDown(KeyCode.Space) ) {
                Player.JumpInput();
            }
        }

        void IFixedUpdate.OnFixedUpdate() {
            if ( !Player ) {
                return;
            }
            Player.OnUpdate();
        }

        void OnPointerDown(TapZone_PointerDown e) {
            if ( !Player ) {
                return;
            }
            Player.JumpInput();
        }
    }
}