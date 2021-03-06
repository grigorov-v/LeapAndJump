﻿using System.Collections.Generic;
using System.Linq;
using Grigorov.Extensions;
using Grigorov.LeapAndJump.Events;
using Grigorov.LeapAndJump.ResourcesContainers;
using Grigorov.Unity.Events;
using UnityEngine;

namespace Grigorov.LeapAndJump.Level {
	[RequireComponent(typeof(LevelGrind))]
	public class LevelElementsGenerator : MonoBehaviour {
		readonly List<LevelElement> _allElements = new List<LevelElement>();
		
		LevelGrind _levelGrind;

		LevelGrind LevelGrind => this.GetComponent(ref _levelGrind);

		void OnDrawGizmos() {
			if ( !LevelGrind ) {
				return;
			}

			GetComponentsInChildren(false, _allElements);
			var boundsArray = LevelGrind.Cells;
			for ( var y = 0; y < boundsArray.GetLength(1); y++ ) {
				for ( var x = 0; x < boundsArray.GetLength(0); x++ ) {
					var cellBounds = boundsArray[x, y];
					Gizmos.color = IsIntersectsWithElement(boundsArray[x, y]) ? Color.red : Color.green;
					Gizmos.DrawWireCube(cellBounds.center, cellBounds.size);
				}
			}
		}

		public void Generate(LevelElementsContainer elementsGroup) {
			GetComponentsInChildren(false, _allElements);

			var rowCount = LevelGrind.Cells.GetLength(1);
			for ( var rowIndex = 1; rowIndex < rowCount; rowIndex++ ) {
				var gridRow = GetGridRow(rowIndex);
				if ( gridRow.Exists(IsIntersectsWithElement) ) {
					continue;
				}

				var nextGridRow = GetGridRow(rowIndex - 1);
				if ( nextGridRow.Exists(IsIntersectsWithElement) ) {
					continue;
				}

				bool trySpawnElement;
				do {
					trySpawnElement = TrySpawnElement(elementsGroup.GetRandomObject(), gridRow);
				} while ( trySpawnElement );

				elementsGroup.RandomizeObjects.ForEach(element => TrySpawnElement(element, gridRow));
			}
		}

		bool TrySpawnElement(LevelElement prefabElement, List<Bounds> gridRow) {
			if ( !prefabElement ) {
				return false;
			}

			var findPos = FindRandomPosition(prefabElement.Bounds, gridRow, out var position);
			if ( !findPos ) {
				return false;
			}

			var elementBounds = prefabElement.Bounds;
			elementBounds.center = position;
			if ( IsIntersectsWithElement(elementBounds) ) {
				return false;
			}

			var element = Instantiate(prefabElement, transform);
			Transform elementTransform;
			var localCenter = (Vector2)(elementTransform = element.transform).InverseTransformPoint(element.Bounds.center);
			position -= localCenter;
			elementTransform.position = position;
			element.TryMirror();
			_allElements.Add(element);
			EventManager.Fire(new SpawnLevelElementEvent(element));
			return true;
		}

		bool IsIntersectsWithElement(Bounds bounds) {
			foreach ( var elem in _allElements ) {
				if ( elem.Bounds.Intersects(bounds) ) {
					return true;
				}
			}

			return false;
		}

		List<Bounds> GetGridRow(int rowIndex) {
			var boundsList = new List<Bounds>();
			var boundsArray = LevelGrind.Cells;
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

				if ( i < gridRow.Count - 1 ) {
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
	}
}