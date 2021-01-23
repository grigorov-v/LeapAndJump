using System.Collections.Generic;
using System.Linq;
using Grigorov.Extensions;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.ResourcesContainers;
using Grigorov.Unity.Controllers;
using Grigorov.Unity.Events;
using UnityEngine;

namespace Grigorov.LeapAndJump.Controllers {
	public sealed class LevelGenerateController : IController {
		const int MinCountBlocks = 4;
		readonly List<LevelBlock> _activeLevelBlocks = new List<LevelBlock>();
		List<LevelElementsContainer> _elementsGroups = new List<LevelElementsContainer>();

		LevelBlocksContainer _levelBlocks;
		Stack<LevelElementsContainer> _stackElementsGroups = new Stack<LevelElementsContainer>();

		LevelController LevelController => ControllersBox.Get<LevelController>();
		LevelBlock LastBlock => _activeLevelBlocks.LastOrDefault();
		LevelBlock FirstBlock => _activeLevelBlocks.FirstOrDefault();

		Stack<LevelElementsContainer> StackElementsGroups {
			get {
				if ( _stackElementsGroups.Count == 0 ) {
					_stackElementsGroups = new Stack<LevelElementsContainer>(_elementsGroups.Randomize());
				}

				return _stackElementsGroups;
			}
		}

		public void OnInit() {
			var sc = ControllersBox.Get<ScenesController>();
			if ( !sc.IsActiveWorldScene ) {
				return;
			}

			_stackElementsGroups.Clear();
			_activeLevelBlocks.Clear();

			var lcc = ControllersBox.Get<LevelConfigController>();
			var levelId = LevelController.CurrentLevel;

			_elementsGroups = lcc.Config.GetElementsGroups(levelId.Level);
			_levelBlocks = lcc.Config.LevelBlocks;

			for ( var i = 0; i < MinCountBlocks; i++ ) {
				CreateNewBlock(false);
			}

			EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);

			Debug.Log(typeof(LevelGenerateController).ToString());
		}

		public void OnReset() {
			EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
		}

		void CreateNewBlock(bool isWinBlock) {
			var newBlock = _levelBlocks.GetRandomObject(false, block => block.IsWinBlock == isWinBlock);
			newBlock = Object.Instantiate(newBlock);
			newBlock.SetPosition(LastBlock);
			newBlock.GenerateLevelElements(StackElementsGroups.Pop());
			AddBlock(newBlock);

			for ( var i = 0; i < _activeLevelBlocks.Count; i++ ) {
				var factor = i + 1;
				_activeLevelBlocks[i].SetBackOrderLayer(factor);
			}
		}

		void AddBlock(LevelBlock levelBlock) {
			if ( _activeLevelBlocks.Exists(block => block == levelBlock) ) {
				return;
			}

			_activeLevelBlocks.Add(levelBlock);
		}

		void RemoveBlock(LevelBlock levelBlock) {
			_activeLevelBlocks.Remove(levelBlock);
		}

		void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e) {
			if ( LastBlock && LastBlock == e.LevelBlock && !e.LevelBlock.IsWinBlock ) {
				var firstBlock = FirstBlock;
				RemoveBlock(firstBlock);
				Object.Destroy(firstBlock.gameObject);
				CreateNewBlock(LevelController.IsLevelFinish);
			}
		}
	}
}