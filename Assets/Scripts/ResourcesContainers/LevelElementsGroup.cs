using UnityEngine;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.ResourcesContainers
{
	[CreateAssetMenu(menuName = "Create ElementsGroup", fileName = "ElementsGroup_Difficulty_{0}")]
	public class LevelElementsGroup : BaseResourcesContainer<LevelElement>
	{
		public static LevelElementsGroup Load(string folderName, string resName)
		{
			return Resources.Load<LevelElementsGroup>($"Elements_Groups/{folderName}/{resName}");
		}
	}
}