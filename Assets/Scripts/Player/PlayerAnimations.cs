using UnityEngine;

using Grigorov.Extensions;
using Grigorov.LeapAndJump.Animations.Wrappers;

using System.Collections.Generic;

using NaughtyAttributes;

namespace Grigorov.LeapAndJump.Animations
{
	public enum KeyAnim
	{
		None,
		Walk,
		Jump,
		SlideInWall,
		SecondJump,
		Idle
	}

	[System.Serializable]
	public struct AnimInfo
	{
		public KeyAnim Key;
		public string  StateName;
	}

	public class PlayerAnimations : MonoBehaviour
	{ 
		[ReorderableList][SerializeField] List<AnimInfo> _animations = new List<AnimInfo>();

		SkeletonAnimationWrapper _skeletonAnimation = null;
		KeyAnim                  _curAnim           = KeyAnim.None;

		SkeletonAnimationWrapper AnimationWrapper => this.GetComponentInChildren(ref _skeletonAnimation);

		public void PlayAnimation(KeyAnim key)
		{
			if (_curAnim == key)
			{
				return;
			}

			var animName = _animations.Find(anim => anim.Key == key).StateName;
			var loop = (key == KeyAnim.Walk) || (key == KeyAnim.SecondJump) ||
											  (key == KeyAnim.SlideInWall) || (key == KeyAnim.Idle);

			AnimationWrapper.SetupAnimation(animName, loop);
			_curAnim = key;
		}
	}
}