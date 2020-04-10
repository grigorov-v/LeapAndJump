using System;
using UnityEngine;
using EventsHelper;

[RequireComponent(typeof(Rigidbody2D))]
public class DestructionObject : MonoBehaviour {
    public float Hp = 2;

    Rigidbody2D  _rb        = null;
    Collider2D[] _colliders = null;
    float        _curHp     = 0;

    void Awake() {
        _rb = GetComponent<Rigidbody2D>();   
        _rb.isKinematic = true;
        _curHp = Hp;
        _colliders = GetComponentsInChildren<Collider2D>();

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
        }
        Debug.Log("!!!");
    }
}