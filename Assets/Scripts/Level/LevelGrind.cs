using UnityEngine;

namespace Grigorov.LeapAndJump.Level {
	public class LevelGrind : MonoBehaviour {
		[SerializeField] Vector2 _cellSize = new Vector2(1.29f, 1.29f);
		[SerializeField] Vector2 _startCenter = new Vector2(-1.93f, -5.87f);
		[SerializeField] Vector2Int _cellCount = new Vector2Int(4, 10);
		Bounds[,] _cells;
		Vector2Int _lastCellCount = Vector2Int.zero;

		Vector2 _lastCellSize = Vector2.zero;
		Vector2 _lastStartCenter = Vector2.zero;

		public Bounds[,] Cells {
			get {
				if ( _cells == null || CheckChanged() ) {
					GrindGenerate();
				}

				return _cells;
			}
		}

		void GrindGenerate() {
			_cells = new Bounds[_cellCount.x, _cellCount.y];
			for ( var y = 0; y < _cellCount.y; y++ ) {
				for ( var x = 0; x < _cellCount.x; x++ ) {
					var center = transform.TransformPoint(_startCenter);
					center.x += _cellSize.x * x;
					center.y += _cellSize.y * y;
					_cells[x, y] = new Bounds(center, _cellSize);
				}
			}

			_lastCellSize = _cellSize;
			_lastStartCenter = _startCenter;
			_lastCellCount = _cellCount;
		}

		bool CheckChanged() {
			return _lastCellSize != _cellSize || _lastStartCenter != _startCenter || _lastCellCount != _cellCount;
		}
	}
}