using UnityEngine;

using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.ResourcesContainers
{
	[CreateAssetMenu(menuName = "Create Foods container", fileName = "Foods")]
	public class Foods : BaseResourcesContainer<Food>
	{
		public static Foods Load(string folderName, string resName)
		{
			return Resources.Load<Foods>($"{folderName}/{resName}");
		}
	}
}