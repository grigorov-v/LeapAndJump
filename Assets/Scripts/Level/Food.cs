using System;
using UnityEngine;

using Grigorov.Events;
using Grigorov.LeapAndJump.Level;

using DG.Tweening;

namespace Grigorov.LeapAndJump.Level {
    public struct FoodCollectEvent {}

    [RequireComponent(typeof(Rigidbody2D))]
    public class Food : BaseLevelElement {
        [SerializeField] float _tweenDuration = 0.5f;

        bool  _isCollecting = false;
        Tween _tweenCollect = null;
        Rigidbody2D _rb     = null;

        Rigidbody2D Rigidbody {
            get {
                if ( !_rb ) {
                   _rb = GetComponent<Rigidbody2D>(); 
                }
                return _rb;
            }
        }
        
        void StartCollect() {
            ResetTween();
            
            Rigidbody.isKinematic = true;
            var colliders = new Collider2D[Rigidbody.attachedColliderCount];
            Rigidbody.GetAttachedColliders(colliders);
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
            var player = other.GetComponent<Player>();
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