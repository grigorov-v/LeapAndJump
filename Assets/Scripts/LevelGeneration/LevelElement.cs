using System.Collections.Generic;
using UnityEngine;

public class LevelElement : MonoBehaviour {
    List<SpriteRenderer> _spritesRenderer = new List<SpriteRenderer>();
    int                  _lastCountChilds = -1;

    public Bounds CommonBounds {
        get {
            var size = new Vector3(TopRightPoint.x - BottomLeftPoint.x, TopRightPoint.y - BottomLeftPoint.y);
            return new Bounds(Center, size);
        }
    }

    List<Bounds> ListBounds {
        get {
            if ( (transform.childCount != _lastCountChilds) || (_spritesRenderer.Count == 0) ) {
                GetComponentsInChildren<SpriteRenderer>(false, _spritesRenderer);
                _lastCountChilds = transform.childCount;
            }
            
            var listBounds = new List<Bounds>();
            _spritesRenderer.ForEach(sr => listBounds.Add(sr.bounds));
            return listBounds;
        }
    }

    Vector2 TopRightPoint {
        get {
            var maxX = 0f;
            var maxY = 0f;
            for ( var i = 0; i < ListBounds.Count; i++ ) {
                var bounds = ListBounds[i];
                var x = bounds.center.x + bounds.extents.x;
                var y = bounds.center.y + bounds.extents.y;
                if ( i == 0 ) {
                    maxX = x;
                    maxY = y;
                    continue;
                }

                maxX = (x > maxX) ? x : maxX;
                maxY = (y > maxY) ? y : maxY;
            }

            return new Vector2(maxX, maxY);
        }
    }

    Vector2 BottomLeftPoint {
        get {
            var minX = 0f;
            var minY = 0f;
            for ( var i = 0; i < ListBounds.Count; i++ ) {
                var bounds = ListBounds[i];
                var x = bounds.center.x - bounds.extents.x;
                var y = bounds.center.y - bounds.extents.y;
                if ( i == 0 ) {
                    minX = x;
                    minY = y;
                    continue;
                }

                minX = (x < minX) ? x : minX;
                minY = (y < minY) ? y : minY;
            }

            return new Vector2(minX, minY);
        }
    }

    Vector2 Center {
        get {
            var x = (BottomLeftPoint.x + TopRightPoint.x) / 2;
            var y = (BottomLeftPoint.y + TopRightPoint.y) / 2;
            return new Vector2(x, y);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(CommonBounds.center, CommonBounds.size);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(Center, 0.05f);
    }
}