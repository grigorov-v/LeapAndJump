using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace Grigorov.LeapAndJump.Animations.Wrappers {
	[RequireComponent(typeof(SkeletonAnimation))]
	public class SkeletonAnimationWrapper : BaseAnimationWrapper<SkeletonAnimation> {
		MeshRenderer _meshRenderer;

		public bool IsAnimationStateActive => AnimationState != null;

		public override AnimationState AnimationState => Animation.AnimationState;

		protected override float TimeScale {
			get => Animation.timeScale;
			set => Animation.timeScale = value;
		}

		protected override SkeletonDataAsset SkeletonDataAsset {
			get => Animation.skeletonDataAsset;
			set => Animation.skeletonDataAsset = value;
		}

		MeshRenderer MeshRenderer {
			get {
				if ( !_meshRenderer ) {
					_meshRenderer = Animation.GetComponent<MeshRenderer>();
				}

				return _meshRenderer;
			}
		}

		public override void SetupSortingLayer(string layer, int order) {
			var renderer = MeshRenderer;
			renderer.sortingLayerName = layer;
			renderer.sortingOrder = order;
		}

		public override void SetupSkin(string skinName) {
			if ( Animation.initialSkinName == skinName ) {
				return;
			}

			Animation.initialSkinName = skinName;
			Initialize(true);
		}

		protected override void Initialize(bool overwrite) {
			Animation.Initialize(true);
		}
	}
}