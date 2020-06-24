using UnityEngine;

using Grigorov.LeapAndJump.Player;

namespace Grigorov.LeapAndJump.CameraManagement {
    public class CameraMovement : MonoBehaviour {
        [SerializeField] float     _lerp      = 3;
        [SerializeField] Vector2   _offset    = new Vector2(0, 0.1f);
        [SerializeField] Transform _edgePoint = null;
        
        Transform _player = null;
        Camera    _camera = null;

        void Awake() {
            _player = FindObjectOfType<PlayerControl>()?.transform;
            _camera = GetComponent<Camera>();
        }

        void FixedUpdate() {
            transform.position = CalculateCameraPosition();
        }

        Vector3 CalculateCameraPosition() {
            var curPos = transform.position;
            curPos = Vector2.Lerp(curPos, _player.position, _lerp * Time.deltaTime);
            curPos += (Vector3)_offset;
            curPos.z = -10;
            curPos.x = 0;
            curPos.y = Mathf.Clamp(curPos.y, CalculateMinYPos(), CalculateMaxYPos());
            return curPos;
        }

        float CalculateMinYPos() {
            var bottomLeft = _camera.ViewportToWorldPoint(new Vector2 (0, 0));
            var minYPos = _edgePoint.position.y + (transform.position.y - bottomLeft.y);
            return minYPos;
        }

        float CalculateMaxYPos() {
            var bottomLeft = _camera.ViewportToWorldPoint(new Vector2 (0, 0));
            var topLeft = _camera.ViewportToWorldPoint(new Vector2 (0, 1));
            var cameraYScale = topLeft.y - bottomLeft.y;
            var maxYPos = _player.position.y + (cameraYScale / 2);
            return maxYPos;
        }
    }
}