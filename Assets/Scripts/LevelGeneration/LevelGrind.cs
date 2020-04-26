using UnityEngine;

public class LevelGrind : MonoBehaviour {
    public Vector2    CellSize    = Vector2.zero;
    public Vector2    StartCenter = Vector2.zero;
    public Vector2Int CellCount   = Vector2Int.zero;

    Vector2    _lastCellSize    = Vector2.zero;
    Vector2    _lastStartCenter = Vector2.zero;
    Vector2Int _lastCellCount   = Vector2Int.zero;

    Bounds[,] _boundsArray = null;

    public Bounds[,] BoundsArray {
        get {
            if ( (_boundsArray == null) || CheckChanged() ) {
                GrindGenerate();
            }
            return _boundsArray;
        }
    }

    void GrindGenerate() {
        _boundsArray = new Bounds[CellCount.x, CellCount.y];
        for ( var y = 0; y < CellCount.y; y++ ) {
            for ( var x = 0; x < CellCount.x; x++ ) {
                var center = transform.TransformPoint(StartCenter);
                center.x += CellSize.x * x;
                center.y += CellSize.y * y;
                _boundsArray[x,y] = new Bounds(center, CellSize);
            }
        }

        _lastCellSize = CellSize;
        _lastStartCenter = StartCenter;
        _lastCellCount = CellCount;
    }

    bool CheckChanged() {
        return (_lastCellSize != CellSize) || (_lastStartCenter != StartCenter) || (_lastCellCount != CellCount);
    }
}