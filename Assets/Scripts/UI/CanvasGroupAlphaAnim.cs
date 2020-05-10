using UnityEngine;

using DG.Tweening;

namespace Grigorov.LeapAndJump.UI {
	[RequireComponent(typeof(CanvasGroup))]
	public sealed class CanvasGroupAlphaAnim : MonoBehaviour {
		public float Delay    = 0;
		public float Duration = 1;
		public float Sleep    = 0;
		public float Hold     = 0;

		CanvasGroup _group = null;
		Sequence    _seq   = null;

		void OnEnable() {
			_group = GetComponent<CanvasGroup>();
			if ( !_group ) {
				return;
			}

			if ( Delay > 0 ) {
				_group.alpha = 0;
				KillSequence();
				_seq = DOTween.Sequence();
				_seq.AppendInterval(Delay);
				_seq.AppendCallback(StartAnim);
			} else {
				StartAnim();
			}
		}

		void OnDisable() {
			StopAnim();
		}

		void StartAnim() {
			_group.alpha = 0;
			KillSequence();
			_seq = DOTween.Sequence();
			_seq.Append(_group.DOFade(1, Duration));
			_seq.AppendInterval(Hold);
			_seq.Append(_group.DOFade(0, Duration));
			_seq.AppendInterval(Sleep);
			_seq.SetLoops(-1);
		}

		void StopAnim() {
			KillSequence();
			_group.alpha = 0;
		}

		void KillSequence() {
			if ( _seq == null ) {
				return;
			}

			_seq.Kill();
			_seq = null;
		}
	}
}