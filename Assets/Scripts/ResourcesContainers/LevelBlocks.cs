using UnityEngine;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.ResourcesContainers
{
	[CreateAssetMenu(menuName = "Create LevelBlocks", fileName = "LevelBlocks")]
	public class LevelBlocks : BaseResourcesContainer<LevelBlock>
	{
		public static LevelBlocks Load(string folderName, string resName)
		{
			return Resources.Load<LevelBlocks>($"{folderName}/{resName}");
		}
	}
}