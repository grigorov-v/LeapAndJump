using UnityEngine;

namespace Game.CameraManagement {
    public class CameraScaler : MonoBehaviour {
        [SerializeField]
        Vector2      _defaultResolution = new Vector2(720, 1280);
        
        [Range(0f, 1f)] [SerializeField]
        public float _widthOrHeight     = 0;

        Camera _camera        = null;
        float  _initialSize   = 0;
        float  _targetAspect  = 0;
        float  _initialFov    = 0;
        float  _horizontalFov = 120f;

        void Start() {
            _camera = GetComponent<Camera>();
            _initialSize = _camera.orthographicSize;
            _targetAspect = _defaultResolution.x / _defaultResolution.y;
            _initialFov = _camera.fieldOfView;
            _horizontalFov = CalcVerticalFov(_initialFov, 1 / _targetAspect);
        }

        void Update() {
            if (_camera.orthographic) {
                var constantWidthSize = _initialSize * (_targetAspect / _camera.aspect);
                _camera.orthographicSize = Mathf.Lerp(constantWidthSize, _initialSize, _widthOrHeight);
            } else {
                var constantWidthFov = CalcVerticalFov(_horizontalFov, _camera.aspect);
                _camera.fieldOfView = Mathf.Lerp(constantWidthFov, _initialFov, _widthOrHeight);
            }
        }

        float CalcVerticalFov(float hFovInDeg, float aspectRatio) {
            var hFovInRads = hFovInDeg * Mathf.Deg2Rad;
            var vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);
            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}