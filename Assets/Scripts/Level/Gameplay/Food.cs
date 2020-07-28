using System;
using UnityEngine;

using Grigorov.Events;
using Grigorov.LeapAndJump.Player;

using DG.Tweening;

namespace Grigorov.LeapAndJump.Level.Gameplay {
    public struct FoodCollectEvent {}

    [RequireComponent(typeof(Rigidbody2D))]
    public class Food : BaseLevelElement {
        [SerializeField] float _tweenDuration = 0.5f;

        bool  _isCollecting = false;
        Tween _tweenCollect = null;
        Rigidbody2D _rb     = null;

        void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        void StartCollect() {
            ResetTween();
            
            _rb.isKinematic = true;
            var colliders = new Collider2D[_rb.attachedColliderCount];
            _rb.GetAttachedColliders(colliders);
            Array.ForEach(colliders, coll => coll.enabled = false);

            _tweenCollect = transform.DOScale(Vector2.zero, _tweenDuration);
            _tweenCollect.onComplete += () => {
                if ( gameObject ) {
                    Destroy(gameObject);
                }
            };

            _isCollecting = true;
        }

        void FinishCollect() {
            if ( !_isCollecting ) {
                return;
            }
            print("Collect");
            EventManager.Fire(new FoodCollectEvent());
        }

        void ResetTween() {
            if ( _tweenCollect == null ) {
                return;
            }
            _tweenCollect.Kill();
            _tweenCollect = null;
        }

        void OnTriggerEnter2D(Collider2D other) {
            var player = other.GetComponent<PlayerControl>();
            if ( player ) {
                StartCollect();
            }
        }

        void OnDestroy() {  
            ResetTween();
            FinishCollect();
        }
    }
}