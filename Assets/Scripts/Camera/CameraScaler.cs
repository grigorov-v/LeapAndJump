using UnityEngine;

public class CameraScaler : MonoBehaviour {
    public Vector2 DefaultResolution = new Vector2(720, 1280);
    
    [Range(0f, 1f)] 
    public float   WidthOrHeight     = 0;

    Camera _camera        = null;
    float  _initialSize   = 0;
    float  _targetAspect  = 0;
    float  _initialFov    = 0;
    float  _horizontalFov = 120f;

    void Start() {
        _camera = GetComponent<Camera>();
        _initialSize = _camera.orthographicSize;
        _targetAspect = DefaultResolution.x / DefaultResolution.y;
        _initialFov = _camera.fieldOfView;
        _horizontalFov = CalcVerticalFov(_initialFov, 1 / _targetAspect);
    }

    void Update() {
        if (_camera.orthographic) {
            var constantWidthSize = _initialSize * (_targetAspect / _camera.aspect);
            _camera.orthographicSize = Mathf.Lerp(constantWidthSize, _initialSize, WidthOrHeight);
        } else {
            var constantWidthFov = CalcVerticalFov(_horizontalFov, _camera.aspect);
            _camera.fieldOfView = Mathf.Lerp(constantWidthFov, _initialFov, WidthOrHeight);
        }
    }

    float CalcVerticalFov(float hFovInDeg, float aspectRatio) {
        var hFovInRads = hFovInDeg * Mathf.Deg2Rad;
        var vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);
        return vFovInRads * Mathf.Rad2Deg;
    }
}