using System.Collections.Generic;
using UnityEngine;

using Grigorov.LeapAndJump.Events;

using EventsHelper;
using NaughtyAttributes;

namespace Grigorov.LeapAndJump.Level {
    public class LevelGenerator : MonoBehaviour {
        [SerializeField] int                _minCountBlocks = 4;
        [SerializeField] List<LevelBlock>   _blocks         = new List<LevelBlock>();
        
        [Space]
        [SerializeField] List<LevelElement> _levelElements  = new List<LevelElement>();

        List<LevelBlock> _activeBlocks = new List<LevelBlock>();

        LevelBlock LastBlock {
            get {
                if ( _activeBlocks.Count == 0 ) {
                    return null;
                }
                
                return _activeBlocks[_activeBlocks.Count - 1];
            }
        }

        void Awake() {
            for ( var i = 0; i < _minCountBlocks; i++ ) {
                CreateNewBlock();
            }

            EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
        }

        void OnDestroy() {
            EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
        }

        void Update() {
            if ( Input.GetKeyDown(KeyCode.G) ) {
                Test();
            }
        }

        void CreateNewBlock() {
            var rand = Random.Range(0, _blocks.Count);
            
            var newBlock = _blocks[rand];
            newBlock = Instantiate(newBlock);
        
            var position = Vector2.zero;
            if ( LastBlock ) {
                position = LastBlock.EndPoint.position - newBlock.BeginPoint.localPosition;
            }

            newBlock.transform.position = position;
            newBlock.GenerateLevelElements(_levelElements);
            _activeBlocks.Add(newBlock);
        }

        void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e) {
            if ( LastBlock && (LastBlock == e.LevelBlock) ) {
                var firstBlock = _activeBlocks[0];
                _activeBlocks.Remove(firstBlock);
                Destroy(firstBlock.gameObject);

                CreateNewBlock();
            }
        }

        void Test() {
            var firstBlock = _activeBlocks[0];
            _activeBlocks.Remove(firstBlock);
            Destroy(firstBlock.gameObject);
            CreateNewBlock();
        }
    }
}