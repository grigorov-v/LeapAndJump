﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Grigorov.LeapAndJump.Level {
    public abstract class BaseLevelElement : MonoBehaviour {
        List<SpriteRenderer> _spritesRenderers = new List<SpriteRenderer>();

        public Bounds Bounds {
            get {
                if ( SpritesRenderers.Count == 1 ) {
                    return SpritesRenderers.First().bounds;
                }
                
                var x = (TopRightPoint.x - BottomLeftPoint.x);
                var y = (TopRightPoint.y - BottomLeftPoint.y);
                var size = new Vector3(x, y);
                return new Bounds(Center, size);
            }
        }

        List<Bounds> ListBounds {
            get {            
                var listBounds = new List<Bounds>();
                SpritesRenderers.ForEach(sr => listBounds.Add(sr.bounds));
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

        List<SpriteRenderer> SpritesRenderers {
            get {
                if ( _spritesRenderers.Count == 0 ) {
                    GetComponentsInChildren<SpriteRenderer>(false, _spritesRenderers);
                }
                return _spritesRenderers;
            }
        }

        void OnDrawGizmos() {
            SpritesRenderers.RemoveAll(sr => !sr);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Bounds.center, 0.05f);
        }
    }
}