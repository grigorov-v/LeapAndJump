using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Grigorov.Extensions;
using Grigorov.Events;
using Grigorov.Controllers;
using Grigorov.LeapAndJump.Events;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.Level.Gameplay;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Controllers {
    public class LevelGenerateController : IInit, IReinit, IAwake {
        const int MinCountBlocks = 4;

        LevelBlocks              _levelBlocks    = null;
        List<LevelElementsGroup> _elementsGroups = new List<LevelElementsGroup>();
        Foods                    _foods          = null;

        Stack<LevelElementsGroup> _stackElementsGroups = new Stack<LevelElementsGroup>();
        List<LevelBlock>          _activeBlocks        = new List<LevelBlock>();

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

        void IInit.OnInit() {
            EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
        }

        void IAwake.OnAwake() {
            var sc = ControllersRegister.FindController<ScenesController>();
            if ( !sc.IsActiveWorldScene ) {
                return;
            }

            _elementsGroups.Clear();
            _stackElementsGroups.Clear();
            _activeBlocks.Clear();

            var world = sc.CurrentSceneName;
            var balanceController = ControllersRegister.FindController<BalanceController>();
            var level = new LevelId(world, 0);
            var allGroupsNames = balanceController.GetElementsGroups(level);
           
            _levelBlocks = LevelBlocks.Load("LevelBlocks", $"{world}_LevelBlocks");
            _foods = Foods.Load("Foods", $"{world}_Foods");
            foreach ( var groupName in allGroupsNames ) {
                var element = Resources.Load<LevelElementsGroup>($"Elements_Groups/{world}/{groupName}");
                _elementsGroups.Add(element);
            }

            for ( var i = 0; i < MinCountBlocks; i++ ) {
                CreateNewBlock();
            }
        }

        void IReinit.OnReinit() {
            EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
        }

        void CreateNewBlock() {
            var newBlock = _levelBlocks.GetRandomObject(notRepetitive: false);
            newBlock = GameObject.Instantiate(newBlock);
            newBlock.SetPosition(LastBlock);
            newBlock.GenerateLevelElements(StackElementsGroups.Pop(), _foods);
            _activeBlocks.Add(newBlock);

            for ( var i = 0; i < _activeBlocks.Count; i++ ) {
                var factor = i + 1;
                _activeBlocks[i].SetBackOrderLayer(factor);
            }
        }

        void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e) {
            if ( LastBlock && (LastBlock == e.LevelBlock) ) {
                var firstBlock = _activeBlocks.First();
                _activeBlocks.Remove(firstBlock);
                GameObject.Destroy(firstBlock.gameObject);
                CreateNewBlock();
            }
        }
    }
}