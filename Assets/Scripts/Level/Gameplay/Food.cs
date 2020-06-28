using System.Collections.Generic;
using UnityEngine;

using Grigorov.Extensions;
using Grigorov.LeapAndJump.Player;

namespace Grigorov.LeapAndJump.Level.Gameplay {
    public class Food : MonoBehaviour {
        [SerializeField] SpriteRenderer _spriteRenderer = null;
        [SerializeField] List<Sprite>   _sprites        = new List<Sprite>();

        void Awake() {
            _spriteRenderer.sprite = _sprites.GetRandomElement();
        }

        void OnTriggerEnter2D(Collider2D other) {
            var player = other.GetComponent<PlayerControl>();
            if ( player ) {
                Destroy(gameObject);
            }
        }
    }
}