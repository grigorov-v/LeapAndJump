using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public abstract class BaseAnimationControl : MonoBehaviour {
    public enum KeyAnim {
        None,
        Walk,
        Jump,
        SlideInWall,
        SecondJump
    }

    [System.Serializable]
    public struct AnimInfo {
        public KeyAnim Key;
        public string  StateName;
    }

    [ReorderableList]
    public List<AnimInfo> Animations = new List<AnimInfo>();

    public abstract void PlayAnimation(KeyAnim key);
}