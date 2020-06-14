using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Grigorov.LeapAndJump.Events;
using Grigorov.Extensions;

using EventsHelper;

namespace Grigorov.LeapAndJump.Level {
    public class LevelGenerator : MonoBehaviour {
        [SerializeField] int              _minCountBlocks = 4;
        [SerializeField] List<LevelBlock> _blocks         = new List<LevelBlock>();
        List<LevelBlock>                  _activeBlocks   = new List<LevelBlock>();
        
        List<ElementsGroup>  _elementsGroups     = new List<ElementsGroup>();
        Stack<ElementsGroup> _stackElementsGroups = new Stack<ElementsGroup>();

        LevelBlock LastBlock {
            get {
                if ( _activeBlocks.Count == 0 ) {
                    return null;
                }
                return _activeBlocks[_activeBlocks.Count - 1];
            }
        }

        void Awake() {
            var world = SceneManager.GetActiveScene().name;
            var balance = new LevelsBalance(world);
            var allGroupsNames = balance.GetElementsGroups(0);
            foreach ( var groupName in allGroupsNames ) {
                var element = Resources.Load<ElementsGroup>($"Elements_Groups/{world}/{groupName}");
                _elementsGroups.Add(element);
            }

            for ( var i = 0; i < _minCountBlocks; i++ ) {
                CreateNewBlock();
            }

            EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
        }

        void OnDestroy() {
            EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
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

            if ( _stackElementsGroups.Count == 0 ) {
                _stackElementsGroups = new Stack<ElementsGroup>(_elementsGroups.Randomize());
            }
            newBlock.GenerateLevelElements(_stackElementsGroups.Pop());

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
    }
}