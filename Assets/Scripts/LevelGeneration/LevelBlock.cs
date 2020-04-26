using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EventsHelper;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelBlock : MonoBehaviour {
    public Transform BeginPoint = null;
    public Transform EndPoint   = null;

    LevelGrind         _levelGrind    = null;
    List<LevelElement> _levelElements = new List<LevelElement>();

    LevelGrind LevelGrind {
        get {
            if ( !_levelGrind ) {
                _levelGrind = GetComponentInChildren<LevelGrind>();
            }
            return _levelGrind;
        }
    }

    List<LevelElement> LevelElements {
        get {
            if ( _levelElements.Count == 0 ) {
                GetComponentsInChildren<LevelElement>(false, _levelElements);
            }

            return _levelElements;
        }
    }

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

    void OnDrawGizmos() {
        if ( !LevelGrind ) {
            return;
        }
        
        var levelElements = LevelElements;
        var boundsArray = LevelGrind.BoundsArray;
        for ( var y = 0; y < LevelGrind.CellCount.y; y++ ) {
            for ( var x = 0; x < LevelGrind.CellCount.x; x++ ) {
                var bounds = boundsArray[x, y];
                var color = levelElements.Exists(elem => elem.CommonBounds.Intersects(bounds)) ? Color.red : Color.green;
                Gizmos.color = color;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }
    }
}