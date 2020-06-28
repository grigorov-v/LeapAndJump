using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Grigorov.Extensions;
using Grigorov.EventsHelper;
using Grigorov.LeapAndJump.Events;
using Grigorov.LeapAndJump.Level.Gameplay;

namespace Grigorov.LeapAndJump.Level {
    public class LevelBlocksGenerator : MonoBehaviour {
        [SerializeField] int              _minCountBlocks = 4;
        [SerializeField] List<LevelBlock> _blocks         = new List<LevelBlock>();
        [SerializeField] List<Food>       _foodPrefabs    = new List<Food>();
        
        List<ElementsGroup>  _elementsGroups      = new List<ElementsGroup>();
        Stack<ElementsGroup> _stackElementsGroups = new Stack<ElementsGroup>();
        List<LevelBlock>     _activeBlocks        = new List<LevelBlock>();

        LevelBlock LastBlock {
            get {
                if ( _activeBlocks.Count == 0 ) {
                    return null;
                }
                return _activeBlocks.Last();
            }
        }

        Stack<ElementsGroup> StackElementsGroups {
            get {
                if ( _stackElementsGroups.Count == 0 ) {
                    _stackElementsGroups = new Stack<ElementsGroup>(_elementsGroups.Randomize());
                }
                return _stackElementsGroups;
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
            newBlock.SetPosition(LastBlock);
            newBlock.GenerateLevelElements(StackElementsGroups.Pop(), _foodPrefabs);
            _activeBlocks.Add(newBlock);
        }

        void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e) {
            if ( LastBlock && (LastBlock == e.LevelBlock) ) {
                var firstBlock = _activeBlocks.First();
                _activeBlocks.Remove(firstBlock);
                Destroy(firstBlock.gameObject);
                CreateNewBlock();
            }
        }
    }
}