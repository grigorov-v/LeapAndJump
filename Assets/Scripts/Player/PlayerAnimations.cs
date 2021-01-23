using System;
using System.Collections.Generic;
using Grigorov.Extensions;
using Grigorov.LeapAndJump.Animations.Wrappers;
using NaughtyAttributes;
using UnityEngine;

namespace Grigorov.LeapAndJump.Animations {
	public enum KeyAnim {
		None,
		Walk,
		Jump,
		SlideInWall,
		SecondJump,
		Idle
	}

	[Serializable]
	public struct AnimInfo {
		public KeyAnim Key;
		public string StateName;
	}

	public class PlayerAnimations : MonoBehaviour {
		[ReorderableList] [SerializeField] List<AnimInfo> _animations = new List<AnimInfo>();
		
		KeyAnim                  _curAnim;
		SkeletonAnimationWrapper _skeletonAnimation;

		SkeletonAnimationWrapper AnimationWrapper => this.GetComponentInChildren(ref _skeletonAnimation);

		public void PlayAnimation(KeyAnim key) {
			if ( _curAnim == key ) {
				return;
			}

			var animName = _animations.Find(anim => anim.Key == key).StateName;
			var loop = key == KeyAnim.Walk || key == KeyAnim.SecondJump ||
			           key == KeyAnim.SlideInWall || key == KeyAnim.Idle;

			AnimationWrapper.SetupAnimation(animName, loop);
			_curAnim = key;
		}
	}
}