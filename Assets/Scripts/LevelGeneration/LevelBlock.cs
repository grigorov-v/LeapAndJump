using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventsHelper;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelBlock : MonoBehaviour {
    public Transform BeginPoint = null;
    public Transform EndPoint   = null;

    void OnValidate() {
        var boxTrigger = GetComponent<BoxCollider2D>();
        if ( !boxTrigger.isTrigger ) {
            boxTrigger.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        var player = other.GetComponent<PlayerControl>();
        if ( player ) {
            EventManager.Fire(new PlayerIntoBlockTriggerEnter(this, player));
        }
    }
}