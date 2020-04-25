using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public float     Lerp      = 10;
    public Vector2   offset    = Vector2.zero;
    public Transform EdgePoint = null;
    
    Transform _player = null;
    Camera    _camera = null;

    void Awake() {
        _player = FindObjectOfType<PlayerControl>()?.transform;
        _camera = GetComponent<Camera>();
    }

    void FixedUpdate() {
        var curPos = transform.position;
        curPos = Vector2.Lerp(curPos, _player.position, Lerp * Time.deltaTime);
        curPos += (Vector3)offset;
        curPos.z = -10;
        curPos.x = 0;
        curPos.y = Mathf.Clamp(curPos.y, CalculateMinYPos(), curPos.y);
        transform.position = curPos;
    }

    float CalculateMinYPos() {
        var bottomLeft = _camera.ScreenToWorldPoint(new Vector2 (0, 0));
        var minYPos = EdgePoint.position.y + (transform.position.y - bottomLeft.y);
        return minYPos;
    }
}