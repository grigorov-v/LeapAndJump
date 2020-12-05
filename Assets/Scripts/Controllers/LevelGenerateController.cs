using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Grigorov.Extensions;
using Grigorov.Unity.Events;
using Grigorov.Unity.Controllers;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Controllers
{
	[Controller]
	public sealed class LevelGenerateController : IInit, IDeinit
	{
		const int MinCountBlocks = 4;

		LevelBlocks               _levelBlocks         = null;
		List<LevelElementsGroup>  _elementsGroups      = new List<LevelElementsGroup>();
		Stack<LevelElementsGroup> _stackElementsGroups = new Stack<LevelElementsGroup>();
		List<LevelBlock>          _activeLevelBlocks   = new List<LevelBlock>();

		LevelController LevelController => Unity.Controllers.ControllersBox.Get<LevelController>();
		LevelBlock      LastBlock       => _activeLevelBlocks.LastOrDefault();
		LevelBlock      FirstBlock      => _activeLevelBlocks.FirstOrDefault();

		Stack<LevelElementsGroup> StackElementsGroups
		{
			get
			{
				if (_stackElementsGroups.Count == 0)
				{
					_stackElementsGroups = new Stack<LevelElementsGroup>(_elementsGroups.Randomize());
				}
				return _stackElementsGroups;
			}
		}

		public void OnInit()
		{
			var sc = ControllersBox.Get<ScenesController>();
			if (!sc.IsActiveWorldScene)
			{
				return;
			}

			_stackElementsGroups.Clear();
			_activeLevelBlocks.Clear();

			var lcc = ControllersBox.Get<LevelConfigController>();
			var levelId = LevelController.CurrentLevel;
			
			_elementsGroups = lcc.Config.GetElementsGroups(levelId.Level);
			_levelBlocks    = lcc.Config.LevelBlocks; 

			for (var i = 0; i < MinCountBlocks; i++)
			{
				CreateNewBlock(false);
			}

			EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);

			UnityEngine.Debug.Log(typeof(LevelGenerateController).ToString());
		}

		public void OnDeinit()
		{
			EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
		}

		void CreateNewBlock(bool isWinBlock)
		{
			var newBlock = _levelBlocks.GetRandomObject(notRepetitive: false, filter: block => block.IsWinBlock == isWinBlock);
			newBlock = GameObject.Instantiate(newBlock);
			newBlock.SetPosition(LastBlock);
			newBlock.GenerateLevelElements(StackElementsGroups.Pop());
			AddBlock(newBlock);

			for (var i = 0; i < _activeLevelBlocks.Count; i++)
			{
				var factor = i + 1;
				_activeLevelBlocks[i].SetBackOrderLayer(factor);
			}
		}

		void AddBlock(LevelBlock levelBlock)
		{
			if (_activeLevelBlocks.Exists(block => block == levelBlock))
			{
				return;
			}
			_activeLevelBlocks.Add(levelBlock);
		}

		void RemoveBlock(LevelBlock levelBlock)
		{
			_activeLevelBlocks.Remove(levelBlock);
		}

		void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e)
		{
			if (LastBlock && (LastBlock == e.LevelBlock) && !e.LevelBlock.IsWinBlock)
			{
				var firstBlock = FirstBlock;
				RemoveBlock(firstBlock);
				GameObject.Destroy(firstBlock.gameObject);
				CreateNewBlock(LevelController.IsLevelFinish);
			}
		}
	}
}