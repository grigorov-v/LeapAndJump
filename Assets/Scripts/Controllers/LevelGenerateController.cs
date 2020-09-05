using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Grigorov.Extensions;
using Grigorov.Events;
using Grigorov.Controllers;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Controllers {
    public class LevelGenerateController : Controller {
        const int MinCountBlocks = 4;

        LevelBlocks              _levelBlocks    = null;
        List<LevelElementsGroup> _elementsGroups = new List<LevelElementsGroup>();

        Stack<LevelElementsGroup> _stackElementsGroups = new Stack<LevelElementsGroup>();
        List<LevelBlock>          _activeBlocks        = new List<LevelBlock>();
        LevelController           _levelController     = null;

        LevelBlock LastBlock {
            get {
                if ( _activeBlocks.Count == 0 ) {
                    return null;
                }
                return _activeBlocks.Last();
            }
        }

        Stack<LevelElementsGroup> StackElementsGroups {
            get {
                if ( _stackElementsGroups.Count == 0 ) {
                    _stackElementsGroups = new Stack<LevelElementsGroup>(_elementsGroups.Randomize());
                }
                return _stackElementsGroups;
            }
        }

        public override void OnInit() {
            _levelController = Controller.FindController<LevelController>();
            EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
        }

        public override void OnAwake() {
            var sc = Controller.FindController<ScenesController>();
            if ( !sc.IsActiveWorldScene ) {
                return;
            }

            _elementsGroups.Clear();
            _stackElementsGroups.Clear();
            _activeBlocks.Clear();

            var bc = Controller.FindController<BalanceController>();
            var level = _levelController.CurrentLevel;
            var allGroupsNames = bc.GetElementsGroups(level);
           
            _levelBlocks = LevelBlocks.Load("LevelBlocks", $"{level.World}_LevelBlocks");
            foreach ( var groupName in allGroupsNames ) {
                var element = Resources.Load<LevelElementsGroup>($"Elements_Groups/{level.World}/{groupName}");
                _elementsGroups.Add(element);
            }

            for ( var i = 0; i < MinCountBlocks; i++ ) {
                CreateNewBlock(false);
            }
        }

        public override void OnDeinit() {
            EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
        }

        void CreateNewBlock(bool isWinBlock) {
            var newBlock = _levelBlocks.GetRandomObject(notRepetitive: false, filter: block => block.IsWinBlock == isWinBlock);
            newBlock = GameObject.Instantiate(newBlock);
            newBlock.SetPosition(LastBlock);
            newBlock.GenerateLevelElements(StackElementsGroups.Pop());
            _activeBlocks.Add(newBlock);

            for ( var i = 0; i < _activeBlocks.Count; i++ ) {
                var factor = i + 1;
                _activeBlocks[i].SetBackOrderLayer(factor);
            }
        }

        void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e) {
            if ( LastBlock && (LastBlock == e.LevelBlock) && !e.LevelBlock.IsWinBlock ) {
                var firstBlock = _activeBlocks.First();
                _activeBlocks.Remove(firstBlock);
                GameObject.Destroy(firstBlock.gameObject);
                CreateNewBlock(_levelController.IsLevelFinish);
            }
        }
    }
}