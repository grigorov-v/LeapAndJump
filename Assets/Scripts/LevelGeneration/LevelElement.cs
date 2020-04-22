using System.Collections.Generic;
using UnityEngine;

public class LevelElement : MonoBehaviour {
    public Vector2 TopRightPoint {
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

    public Vector2 BottomLeftPoint {
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

    public Vector2 TopLeftPoint {
        get {
            return new Vector2(BottomLeftPoint.x, TopRightPoint.y);
        }
    }

    public Vector2 BottomRightPoint {
        get {
            return new Vector2(TopRightPoint.x, BottomLeftPoint.y);
        }
    }

    public Vector2 Center {
        get {
            var x = (BottomLeftPoint.x + TopRightPoint.x) / 2;
            var y = (BottomLeftPoint.y + TopRightPoint.y) / 2;
            return new Vector2(x, y);
        }
    }

    List<Bounds> ListBounds {
        get {
            var spriteRenderers = new List<SpriteRenderer>();
            GetComponentsInChildren<SpriteRenderer>(false, spriteRenderers);
            var listBounds = new List<Bounds>();
            spriteRenderers.ForEach(sr => listBounds.Add(sr.bounds));
            return listBounds;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(TopLeftPoint,     TopRightPoint);
        Gizmos.DrawLine(TopRightPoint,    BottomRightPoint);
        Gizmos.DrawLine(BottomRightPoint, BottomLeftPoint);
        Gizmos.DrawLine(BottomLeftPoint,  TopLeftPoint);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(TopLeftPoint,     0.05f);
        Gizmos.DrawSphere(TopRightPoint,    0.05f);
        Gizmos.DrawSphere(BottomRightPoint, 0.05f);
        Gizmos.DrawSphere(BottomLeftPoint,  0.05f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(Center, 0.05f);
    }
}