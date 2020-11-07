using UnityEngine;
using Spine.Unity;

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
		[SerializeField] SkeletonAnimation _skeletonAnimation = null;

		[ReorderableList]
		[SerializeField] List<AnimInfo> _animations = new List<AnimInfo>();

		KeyAnim _curAnim = KeyAnim.None;

		public void PlayAnimation(KeyAnim key)
		{
			if (_curAnim == key)
			{
				return;
			}

			var animName = _animations.Find(anim => anim.Key == key).StateName;
			_skeletonAnimation.loop = (key == KeyAnim.Walk) || (key == KeyAnim.SecondJump) ||
											  (key == KeyAnim.SlideInWall) || (key == KeyAnim.Idle);

			_skeletonAnimation.AnimationName = animName;
			_curAnim = key;
		}
	}
}