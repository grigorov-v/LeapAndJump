using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Game.Events;
using Game.Player;

using EventsHelper;

namespace Game.Level {
    
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(LevelGrind))]
    public class LevelBlock : MonoBehaviour {
        public Transform BeginPoint = null;
        public Transform EndPoint   = null;

        LevelGrind   _levelGrind          = null;
        List<Bounds> _otherElementsBounds = new List<Bounds>();

        LevelGrind LevelGrind {
            get {
                if ( !_levelGrind ) {
                    _levelGrind = GetComponent<LevelGrind>();
                }
                return _levelGrind;
            }
        }

        void OnValidate() {
            var boxTrigger = GetComponent<BoxCollider2D>();
            if ( !boxTrigger.isTrigger ) {
                boxTrigger.isTrigger = true;
            }
        }

        public void GenerateLevelElements(List<LevelElement> levelElements) {
            var elements = GetComponentsInChildren<LevelElement>();
            foreach ( var elem in elements ) {
                _otherElementsBounds.Add(elem.Bounds);
            }

            var rowCount = LevelGrind.BoundsArray.GetLength(1);
            for ( var rowIndex = 1; rowIndex < rowCount; rowIndex ++ ) {
                var gridRow = GetGridRow(rowIndex);
                if ( gridRow.Exists(cell => IsIntersectsWithElement(cell)) ) {
                    continue;
                }

                var nextGridRow = GetGridRow(rowIndex - 1);
                if ( nextGridRow.Exists(cell => IsIntersectsWithElement(cell)) ) {
                    continue;
                }

                var findPos = false;
                do {
                    var position = Vector2.zero;
                    var randIndex = Random.Range(0, levelElements.Count);
                    var randElement = levelElements[randIndex];
                    findPos = FindRandomPosition(randElement.Bounds, gridRow, out position);
                    if ( !findPos ) {
                        continue;
                    }

                    var elementBounds = randElement.Bounds;
                    elementBounds.center = position;
                    if ( IsIntersectsWithElement(elementBounds) ) {
                        continue;
                    }

                    var element = Instantiate(randElement, transform);
                    var localCenter = (Vector2)element.transform.InverseTransformPoint(element.Bounds.center);
                    position -= localCenter;
                    element.transform.position = position;
                    _otherElementsBounds.Add(elementBounds);
                    
                } while (findPos);
            }  
        }

        bool IsIntersectsWithElement(Bounds bounds) {
            foreach ( var elemBounds in _otherElementsBounds ) {
                if ( elemBounds.Intersects(bounds) ) {
                    return true;
                }
            }

            return false;
        }

        List<Bounds> GetGridRow(int rowIndex) {
            var boundsList = new List<Bounds>();
            var boundsArray = LevelGrind.BoundsArray;
            for ( var i = 0; i < boundsArray.GetLength(0); i++ ) {
                var bounds = boundsArray[i, rowIndex];
                boundsList.Add(bounds);
            }

            return boundsList;
        }

        bool FindRandomPosition(Bounds elementBounds, List<Bounds> gridRow, out Vector2 position) {
            position = Vector2.zero;
            var freeCells = GetFreeCells(elementBounds, gridRow);
            if ( freeCells.Count == 0 ) {
                return false;
            }

            var firstCell = freeCells.First();
            var lastCell = freeCells.Last();
            var minX = firstCell.center.x - firstCell.extents.x + elementBounds.extents.x;
            var maxX = lastCell.center.x + lastCell.extents.x - elementBounds.extents.x;
            var posY = firstCell.center.y - firstCell.extents.y + elementBounds.extents.y;
            position = new Vector2(Random.Range(minX, maxX), posY);
            return true;
        }

        List<Bounds> GetFreeCells(Bounds elementBounds, List<Bounds> gridRow) {
            var freeCells = new List<Bounds>();
            var sumSize = 0f;
            for ( var i = 0; i < gridRow.Count; i++ ) {
                var cell = gridRow[i];
                if ( IsIntersectsWithElement(cell) ) {
                    if ( sumSize >= elementBounds.size.x ) {
                        break;
                    }

                    freeCells.Clear();
                    sumSize = 0f;
                    continue;
                }

                if ( i > 0 ) {
                    var prevCell = gridRow[i - 1];
                    if ( IsIntersectsWithElement(prevCell) ) {
                        continue;
                    }
                }

                if ( i < (gridRow.Count - 1) ) {
                    var nextCell = gridRow[i + 1];
                    if ( IsIntersectsWithElement(nextCell) ) {
                        continue;
                    }
                }

                freeCells.Add(cell);
                sumSize += cell.size.x;
            }

            if ( sumSize < elementBounds.size.x ) {
                freeCells.Clear();
            }

            return freeCells;
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

            _otherElementsBounds.Clear();
            var elements = GetComponentsInChildren<LevelElement>();
            foreach ( var elem in elements ) {
                _otherElementsBounds.Add(elem.Bounds);
            }
            
            var boundsArray = LevelGrind.BoundsArray;
            for ( var y = 0; y < boundsArray.GetLength(1); y++ ) {
                for ( var x = 0; x < boundsArray.GetLength(0); x++ ) {
                    var cellBounds = boundsArray[x, y];
                    Gizmos.color = IsIntersectsWithElement(boundsArray[x, y]) ? Color.red : Color.green;
                    Gizmos.DrawWireCube(cellBounds.center, cellBounds.size);
                }
            }
        }
    }
}