using System;
using System.Collections.Generic;
using Grigorov.LeapAndJump.ResourcesContainers;
using UnityEngine;

namespace Grigorov.LeapAndJump.Level {
	[Serializable]
	public struct LevelInfo {
		public List<LevelElementsContainer> ElementsGroups;
		public int FoodsCount;
	}

	[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig", order = 1)]
	public class LevelConfig : ScriptableObject {
		[SerializeField] LevelBlocksContainer _levelBlocks;
		[SerializeField] FoodsContainer _foods;
		[SerializeField] List<LevelInfo> _levels = new List<LevelInfo>();

		public LevelBlocksContainer LevelBlocks => _levelBlocks;
		public FoodsContainer Foods => _foods;

		public List<LevelElementsContainer> GetElementsGroups(int levelIndex) {
			return CheckLevelIndex(levelIndex) ? _levels[levelIndex].ElementsGroups : null;
		}

		public int GetFoodsCount(int levelIndex) {
			return CheckLevelIndex(levelIndex) ? _levels[levelIndex].FoodsCount : 0;
		}

		bool CheckLevelIndex(int levelIndex) {
			return levelIndex < _levels.Count;
		}

		public static LevelConfig Load(string world) {
			return Resources.Load<LevelConfig>($"LevelConfigs/{world}");
		}
	}
}