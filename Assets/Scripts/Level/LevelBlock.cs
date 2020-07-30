using System.Collections.Generic;
using UnityEngine;

using Grigorov.Events;
using Grigorov.LeapAndJump.Events;
using Grigorov.LeapAndJump.Player;
using Grigorov.LeapAndJump.Level.Gameplay;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Level {
    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelBlock : MonoBehaviour {
        [SerializeField] Transform _beginPoint = null;
        [SerializeField] Transform _endPoint   = null;

        LevelElementsGenerator _elementsGenerator = null;

        void OnValidate() {
            var boxTrigger = GetComponent<BoxCollider2D>();
            if ( !boxTrigger.isTrigger ) {
                boxTrigger.isTrigger = true;
            }
        }

        void Awake() {
            _elementsGenerator = GetComponent<LevelElementsGenerator>();
        }

        public void GenerateLevelElements(LevelElementsGroup elementsGroup, Foods foods) {
            if ( _elementsGenerator ) {
                _elementsGenerator.Generate(elementsGroup, foods);
            }
        }

        public void SetPosition(LevelBlock lastBlock) {
            var position = Vector2.zero;
            if ( lastBlock ) {
                position = lastBlock._endPoint.position - _beginPoint.localPosition;
            }
            transform.position = position;
        }

        void OnTriggerEnter2D(Collider2D other) {
            var player = other.GetComponent<PlayerControl>();
            if ( player ) {
                EventManager.Fire(new PlayerIntoBlockTriggerEnter(this, player));
            }
        }
    }
}