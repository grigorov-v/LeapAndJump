using UnityEngine;

using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.ResourcesContainers
{
	[CreateAssetMenu(menuName = "Create Foods container", fileName = "Foods")]
	public class FoodsContainer : BaseResourcesContainer<Food> {}
}