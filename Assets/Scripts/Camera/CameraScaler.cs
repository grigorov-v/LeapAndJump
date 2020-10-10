using UnityEngine;
using Grigorov.Extensions;

namespace Grigorov.LeapAndJump.CameraManagement {
    public class CameraScaler : MonoBehaviour {
        [SerializeField]
        Vector2 _defaultResolution = new Vector2(720, 1280);
        
        [Range(0f, 1f)] [SerializeField]
        float _widthOrHeight = 0;

        Camera _camera        = null;
        float  _initialSize   = 0;
        float  _targetAspect  = 0;
        float  _initialFov    = 0;
        float  _horizontalFov = 120f;

        Camera Camera => this.GetComponent(ref _camera);

        void Start() {
            _initialSize = Camera.orthographicSize;
            _targetAspect = _defaultResolution.x / _defaultResolution.y;
            _initialFov = Camera.fieldOfView;
            _horizontalFov = CalcVerticalFov(_initialFov, 1 / _targetAspect);
        }

        void Update() {
            if (Camera.orthographic) {
                var constantWidthSize = _initialSize * (_targetAspect / Camera.aspect);
                Camera.orthographicSize = Mathf.Lerp(constantWidthSize, _initialSize, _widthOrHeight);
            } else {
                var constantWidthFov = CalcVerticalFov(_horizontalFov, Camera.aspect);
                Camera.fieldOfView = Mathf.Lerp(constantWidthFov, _initialFov, _widthOrHeight);
            }
        }

        float CalcVerticalFov(float hFovInDeg, float aspectRatio) {
            var hFovInRads = hFovInDeg * Mathf.Deg2Rad;
            var vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);
            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}