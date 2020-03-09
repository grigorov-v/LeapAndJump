using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public float     Lerp    = 10;
    public Transform Player  = null;
    public float     MinYPos = 0;

    public Vector2   offset  = Vector2.zero;

    void Awake() {
        if ( !Player ) {
            Player = FindObjectOfType<PlayerMovement>()?.transform;
        }    
    }

    void FixedUpdate() {
        var curPos = transform.position;
        curPos = Vector2.Lerp(curPos, Player.position, Lerp * Time.deltaTime);
        curPos += (Vector3)offset;
        curPos.z = -10;
        curPos.x = 0;
        curPos.y = Mathf.Clamp(curPos.y, MinYPos, curPos.y);
        transform.position = curPos;
    }
}