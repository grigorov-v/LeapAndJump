using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;

namespace Grigorov.LeapAndJump.Animations.Wrappers {
	[RequireComponent(typeof(SkeletonGraphic))]
	public class SkeletonGraphicsWrapper : BaseAnimationWrapper<SkeletonGraphic> {
		public bool SetupAlphaFromCanvasGroup;

		Canvas _canvas;
		CanvasGroup _canvasGroup;

		public override AnimationState AnimationState => Animation.AnimationState;

		protected override float TimeScale {
			get => Animation.timeScale;
			set => Animation.timeScale = value;
		}

		protected override SkeletonDataAsset SkeletonDataAsset {
			get => Animation.skeletonDataAsset;
			set => Animation.skeletonDataAsset = value;
		}

		Canvas Canvas {
			get {
				if ( !_canvas ) {
					_canvas = Animation.GetComponentInParent<Canvas>();
				}

				return _canvas;
			}
		}

		float Alpha {
			get => Animation.color.a;
			set {
				var color = Animation.color;
				Animation.color = new Color(color.r, color.g, color.b, value);
			}
		}

		protected void Awake() {
			if ( SetupAlphaFromCanvasGroup ) {
				_canvasGroup = GetComponentInParent<CanvasGroup>();
			}
		}

		protected void Update() {
			UpdateAlpha();
		}

		public override void SetupSortingLayer(string layer, int order) {
			var canvas = Canvas;
			canvas.sortingLayerName = layer;
			canvas.sortingOrder = order;
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

		void UpdateAlpha() {
			if ( !SetupAlphaFromCanvasGroup || !_canvasGroup ) {
				return;
			}

			if ( Alpha == _canvasGroup.alpha ) {
				return;
			}

			Alpha = _canvasGroup.alpha;
		}
	}
}