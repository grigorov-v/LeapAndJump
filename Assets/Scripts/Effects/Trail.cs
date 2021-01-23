using NaughtyAttributes;
using UnityEngine;

namespace Grigorov.LeapAndJump.Effects {
	public class Trail : MonoBehaviour {
		[SerializeField] float _lifeTime = -1;
		
		float _lastStartTime = -1;

		TrailRenderer _trailRenderer;

		bool IsPlaying {
			get => _trailRenderer.emitting;
			set => _trailRenderer.emitting = value;
		}

		void Awake() {
			_trailRenderer = GetComponent<TrailRenderer>();
		}

		void Update() {
			if ( _lifeTime < 0 ) {
				return;
			}

			var time = Time.time - _lastStartTime;
			if ( time >= _lifeTime && IsPlaying ) {
				Stop();
			}
		}

		[Button]
		public void Play() {
			_trailRenderer.Clear();
			IsPlaying = true;
			_lastStartTime = Time.time;
		}

		[Button]
		public void Stop() {
			_trailRenderer.Clear();
			IsPlaying = false;
		}
	}
}