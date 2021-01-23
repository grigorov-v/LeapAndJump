using Grigorov.Extensions;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace Grigorov.LeapAndJump.Animations.Wrappers {
	public abstract class BaseAnimationWrapper<T> : MonoBehaviour where T : Component {
		T _animation;
		protected float _defaultTimeScale = 1;

		public abstract AnimationState AnimationState { get; }
		protected abstract float TimeScale { get; set; }
		protected abstract SkeletonDataAsset SkeletonDataAsset { get; set; }

		public T Animation => this.GetComponent(ref _animation);

		protected void Start() {
			_defaultTimeScale = TimeScale;
		}

		public void SetupSkeletonData(SkeletonDataAsset skeletonData) {
			if ( SkeletonDataAsset == skeletonData ) {
				return;
			}

			SkeletonDataAsset = skeletonData;
			Initialize(true);
		}

		public void SetupAnimation(string name, bool loop = false, int trackIndex = 0) {
			var state = AnimationState;
			if ( !CheckAnimationState(state) ) {
				return;
			}

			state.SetAnimation(trackIndex, name, loop);
		}

		public void AddAnimation(string name, bool loop = false, int trackIndex = 0, float delay = 0) {
			var state = AnimationState;
			if ( !CheckAnimationState(state) ) {
				return;
			}

			state.AddAnimation(trackIndex, name, loop, delay);
		}

		public void RewindToStartFrame(int trackIndex = 0) {
			var state = AnimationState;
			if ( !CheckAnimationState(state) ) {
				return;
			}

			var track = state.GetCurrent(trackIndex);
			if ( track == null ) {
				return;
			}

			track.TrackTime = track.AnimationStart;
		}

		public void RewindToEndFrame(int trackIndex = 0) {
			var state = AnimationState;
			if ( !CheckAnimationState(state) ) {
				return;
			}

			var track = state.GetCurrent(trackIndex);
			if ( track == null ) {
				return;
			}

			track.TrackTime = track.AnimationEnd;
		}

		public float GetCurrentTime(int trackIndex = 0) {
			var state = AnimationState;
			if ( !CheckAnimationState(state) ) {
				return -1;
			}

			var track = state.GetCurrent(trackIndex);
			return track == null ? -1 : track.TrackTime;
		}

		public void SetupPause(bool pause) {
			TimeScale = pause ? 0 : _defaultTimeScale;
		}

		public void SetupTimeScale(float scale) {
			_defaultTimeScale = scale;
			TimeScale = scale;
		}

		public abstract void SetupSortingLayer(string layer, int order);
		public abstract void SetupSkin(string skinName);

		protected abstract void Initialize(bool overwrite);

		bool CheckAnimationState(AnimationState state) {
			if ( state == null ) {
				Debug.LogError("AnimationState is null");
				return false;
			}

			return true;
		}
	}
}