using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Grigorov.Events;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Level {
    public struct PlayerIntoBlockTriggerEnter {
        public LevelBlock LevelBlock { get; private set; }
        public Player     Player     { get; private set; }

        public PlayerIntoBlockTriggerEnter (LevelBlock levelBlock, Player player) {
            LevelBlock = levelBlock;
            Player = player;
        }
    }

    [RequireComponent(typeof(BoxCollider2D))]
    public class LevelBlock : MonoBehaviour {
        [SerializeField] Transform _beginPoint = null;
        [SerializeField] Transform _endPoint   = null;

        [SerializeField] List<SpriteRenderer> _backLayers = new List<SpriteRenderer>();

        LevelElementsGenerator _elementsGenerator = null;
        List<int>              _layers            = new List<int>();

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

        public void SetBackOrderLayer(int factor) {
            if ( _layers.Count == 0 ) {
                _layers = _backLayers.Select(bl => bl.sortingOrder).ToList();
            }
            
            for ( var i = 0; i < _backLayers.Count; i++ ) {
                _backLayers[i].sortingOrder = _layers[i] * factor;
            }
        }

        void OnTriggerEnter2D(Collider2D other) {
            var player = other.GetComponent<Player>();
            if ( player ) {
                EventManager.Fire(new PlayerIntoBlockTriggerEnter(this, player));
            }
        }
    }
}