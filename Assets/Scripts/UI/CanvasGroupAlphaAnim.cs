using DG.Tweening;
using UnityEngine;

namespace Grigorov.LeapAndJump.UI {
	[RequireComponent(typeof(CanvasGroup))]
	public sealed class CanvasGroupAlphaAnim : MonoBehaviour {
		public float Delay;
		public float Duration = 1;
		public float Sleep;
		public float Hold;

		CanvasGroup _group;
		Sequence _seq;

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
			}
			else {
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