using UnityEngine;
using Grigorov.LeapAndJump.Player;

namespace Grigorov.LeapAndJump.Level.Gameplay {
    public class Food : BaseLevelElement {
        void OnTriggerEnter2D(Collider2D other) {
            var player = other.GetComponent<PlayerControl>();
            if ( player ) {
                Destroy(gameObject);
            }
        }
    }
}