using UnityEngine;
using System.Collections.Generic;

using Grigorov.LeapAndJump.ResourcesContainers;

using NaughtyAttributes;

namespace Grigorov.LeapAndJump.Level
{
	[System.Serializable]
	public struct LevelInfo
	{
		public List<LevelElementsGroup> ElementsGroups;
		public int FoodsCount;
	}

	[CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig", order = 1)]
	public class LevelConfig : ScriptableObject
	{
		[SerializeField] LevelBlocks     _levelBlocks = null;
		[SerializeField] Foods           _foods       = null;
		[SerializeField] List<LevelInfo> _levels      = new List<LevelInfo>();

		public LevelBlocks LevelBlocks => _levelBlocks;
		public Foods       Foods       => _foods;

		public List<LevelElementsGroup> GetElementsGroups(int levelIndex)
		{
			return CheckLevelIndex(levelIndex) ? _levels[levelIndex].ElementsGroups : null;
		}

		public int GetFoodsCount(int levelIndex)
		{
			return CheckLevelIndex(levelIndex) ? _levels[levelIndex].FoodsCount : 0;
		}

		bool CheckLevelIndex(int levelIndex)
		{
			return levelIndex < _levels.Count;
		}

		public static LevelConfig Load(string world)
		{
			return Resources.Load<LevelConfig>($"LevelConfigs/{world}");
		}
	}
}
