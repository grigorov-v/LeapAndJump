using UnityEngine;

using Grigorov.Extensions;
using System.Collections.Generic;
using Grigorov.LeapAndJump.Animations.Wrappers;
using Grigorov.LeapAndJump.Level;
using NaughtyAttributes;

public class PlayerStateView : MonoBehaviour {
	[System.Serializable]
	 public struct AnimationInfo {
		public PlayerState State;
		public string      AnimationName;
	 }
	
	[ReorderableList] [SerializeField] List<AnimationInfo> _animations = new List<AnimationInfo>();
	
	SkeletonAnimationWrapper _animation;
	
	SkeletonAnimationWrapper AnimationWrapper => this.GetComponentInChildren(ref _animation);
	
	public void Play(PlayerState state) {
		var animName = _animations.Find(anim => anim.State == state).AnimationName;
		var loop = state == PlayerState.Walk || state == PlayerState.SecondJump || state == PlayerState.SlideInWall;
		AnimationWrapper.SetupAnimation(animName, loop);
	}
}
