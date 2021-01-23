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
		
		public bool IsActive => ControllersBox.Get<ScenesController>()?.IsActiveWorldScene ?? false;

		List<LevelBlock>             _activeLevelBlocks;
		List<LevelElementsContainer> _elementsGroups;

		LevelBlocksContainer          _levelBlocks;
		Stack<LevelElementsContainer> _stackElementsGroups;

		LevelController LevelController => ControllersBox.Get<LevelController>();
		LevelBlock      LastBlock       => _activeLevelBlocks.LastOrDefault();
		LevelBlock      FirstBlock      => _activeLevelBlocks.FirstOrDefault();

		Stack<LevelElementsContainer> StackElementsGroups {
			get {
				if ( _stackElementsGroups == null || _stackElementsGroups.Count == 0 ) {
					_stackElementsGroups = new Stack<LevelElementsContainer>(_elementsGroups.Randomize());
				}

				return _stackElementsGroups;
			}
		}

		public void OnInit() {
			_activeLevelBlocks = new List<LevelBlock>();

			var lcc = ControllersBox.Get<LevelConfigController>();
			var levelId = LevelController.CurrentLevel;
			_elementsGroups = lcc.Config.GetElementsGroups(levelId.Level);
			_levelBlocks = lcc.Config.LevelBlocks;

			for ( var i = 0; i < MinCountBlocks; i++ ) {
				CreateNewBlock(false);
			}

			EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
		}

		public void OnReset() {
			EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);

			_activeLevelBlocks = null;
			_elementsGroups = null;
			_levelBlocks = null;
			_stackElementsGroups = null;
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
			if ( !LastBlock || LastBlock != e.LevelBlock || e.LevelBlock.IsWinBlock ) {
				return;
			}

			var firstBlock = FirstBlock;
			RemoveBlock(firstBlock);
			Object.Destroy(firstBlock.gameObject);
			CreateNewBlock(LevelController.IsLevelFinish);
		}
	}
}