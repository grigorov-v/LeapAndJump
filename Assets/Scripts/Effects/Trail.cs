using UnityEngine;

using NaughtyAttributes;

namespace Grigorov.LeapAndJump.Effects {
    public class Trail : MonoBehaviour {
        [SerializeField] float _lifeTime = -1;

        TrailRenderer _trailRenderer = null;
        float         _lastStartTime = -1;

        public bool IsPlaying {
            get => _trailRenderer.emitting;
            private set => _trailRenderer.emitting = value;
        }

        void Awake() {
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        void Update() {
            if ( _lifeTime < 0 ) {
                return;
            }

            var time = Time.time - _lastStartTime;
            if ( (time >= _lifeTime) && IsPlaying ) {
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