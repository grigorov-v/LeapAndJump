using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Grigorov.Extensions;
using Grigorov.Unity.Events;
using Grigorov.Controllers;
using Grigorov.LeapAndJump.Level;
using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Controllers
{
	[Controller]
	public class LevelGenerateController : ControllerForComponent<LevelBlock>, IAwake, IDestroy
	{
		const int MinCountBlocks = 4;

		LevelBlocks               _levelBlocks         = null;
		List<LevelElementsGroup>  _elementsGroups      = new List<LevelElementsGroup>();
		Stack<LevelElementsGroup> _stackElementsGroups = new Stack<LevelElementsGroup>();

		LevelController LevelController => Controller.Get<LevelController>();
		LevelBlock      LastBlock       => _components.LastOrDefault();
		LevelBlock      FirstBlock      => _components.FirstOrDefault();

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

		public void OnAwake()
		{
			var sc = Controller.Get<ScenesController>();
			if (!sc.IsActiveWorldScene)
			{
				return;
			}

			_elementsGroups.Clear();
			_stackElementsGroups.Clear();

			var bc = Controller.Get<BalanceController>();
			var level = LevelController.CurrentLevel;
			var allGroupsNames = bc.GetElementsGroups(level);

			_levelBlocks = LevelBlocks.Load("LevelBlocks", $"{level.World}_LevelBlocks");
			foreach (var groupName in allGroupsNames)
			{
				var element = Resources.Load<LevelElementsGroup>($"Elements_Groups/{level.World}/{groupName}");
				_elementsGroups.Add(element);
			}

			for (var i = 0; i < MinCountBlocks; i++)
			{
				CreateNewBlock(false);
			}

			EventManager.Subscribe<PlayerIntoBlockTriggerEnter>(this, OnPlayerIntoBlockTriggerEnter);
		}

		public void OnDestroy()
		{
			EventManager.Unsubscribe<PlayerIntoBlockTriggerEnter>(OnPlayerIntoBlockTriggerEnter);
		}

		void CreateNewBlock(bool isWinBlock)
		{
			var newBlock = _levelBlocks.GetRandomObject(notRepetitive: false, filter: block => block.IsWinBlock == isWinBlock);
			newBlock = GameObject.Instantiate(newBlock);
			newBlock.SetPosition(LastBlock);
			newBlock.GenerateLevelElements(StackElementsGroups.Pop());
			AddComponent(newBlock);

			for (var i = 0; i < _components.Count; i++)
			{
				var factor = i + 1;
				_components[i].SetBackOrderLayer(factor);
			}
		}

		void OnPlayerIntoBlockTriggerEnter(PlayerIntoBlockTriggerEnter e)
		{
			if (LastBlock && (LastBlock == e.LevelBlock) && !e.LevelBlock.IsWinBlock)
			{
				var firstBlock = FirstBlock;
				RemoveComponent(firstBlock);
				GameObject.Destroy(firstBlock.gameObject);
				CreateNewBlock(LevelController.IsLevelFinish);
			}
		}
	}
}