using System;
using UnityEngine;
using UnityEngine.UI;

using Grigorov.LeapAndJump.Events;

using Grigorov.EventsHelper;
using DG.Tweening;

namespace Grigorov.LeapAndJump.Level {
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class DestructionObject : MonoBehaviour {
        [SerializeField] float _hp = 2;

        Rigidbody2D  _rb         = null;
        Collider2D[] _colliders  = null;
        float        _curHp      = 0;
        Image        _image      = null;
        Tween        _tweenShake = null;

        void Awake() {
            _rb = GetComponent<Rigidbody2D>();   
            _rb.isKinematic = true;
            _curHp = _hp;
            _colliders = GetComponentsInChildren<Collider2D>();
            _image = GetComponent<Image>();

            EventManager.Subscribe<DestructionObjectPlayerCollision>(this, OnDestructionObjectPlayerCollision);
        }

        void OnDestroy() {
            EventManager.Unsubscribe<DestructionObjectPlayerCollision>(OnDestructionObjectPlayerCollision);
        }

        public void Destruct() {
            Array.ForEach(_colliders, coll => coll.enabled = false);
            _rb.isKinematic = false;

            Destroy(gameObject, 4);
        }

        void OnDestructionObjectPlayerCollision(DestructionObjectPlayerCollision e) {
            if ( e.DestructionObject != this ) {
                return;
            }

            _curHp --;
            if ( _curHp <= 0 ) {
                _curHp = 0;
                Destruct();
                return;
            }

            if ( _tweenShake != null ) {
                _tweenShake.Kill();
                _tweenShake = null;
            }
            _tweenShake = transform.DOShakePosition(1, 0.1f);
        }
    }
}