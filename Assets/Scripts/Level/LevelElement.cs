using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

using Grigorov.Extensions;
using Grigorov.LeapAndJump.Level.Gameplay;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Grigorov.LeapAndJump.Level {
    public class LevelElement : MonoBehaviour {
        [SerializeField] List<SpriteRenderer> _spritesRenderers = new List<SpriteRenderer>();
        [SerializeField] List<Transform>      _foodPoints       = new List<Transform>();

        public Bounds Bounds {
            get {
                var x = (TopRightPoint.x - BottomLeftPoint.x);
                var y = (TopRightPoint.y - BottomLeftPoint.y);
                var size = new Vector3(x, y);
                return new Bounds(Center, size);
            }
        }

        List<Bounds> ListBounds {
            get {            
                var listBounds = new List<Bounds>();
                _spritesRenderers.ForEach(sr => listBounds.Add(sr.bounds));
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

        public void SpawnFood(Food food) {
            var randomizePoints = _foodPoints.Randomize();
            foreach ( var point in randomizePoints ) {
                var canSpawn = Random.Range(0, 2) > 0;
                if ( !canSpawn ) {
                    continue;
                }
                Instantiate(food, point.position, food.transform.rotation);
            }
        }

        void OnDrawGizmos() {
            _spritesRenderers.RemoveAll(sr => !sr);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Center, 0.05f);
        }

    #if UNITY_EDITOR
        [Button]
        void FindSpriteRenderers() {
            GetComponentsInChildren<SpriteRenderer>(false, _spritesRenderers);
            PrefabUtility.SavePrefabAsset(gameObject);
        }
    #endif
    }
}