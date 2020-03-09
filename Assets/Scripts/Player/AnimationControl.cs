using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour {
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

    public List<AnimInfo> Animations = new List<AnimInfo>();

    Animator _animator   = null;
    
    public KeyAnim  CurrentKey {get; private set;}

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
    }

    public void PlayAnimation(KeyAnim key, bool force = false) {
        if ( (CurrentKey == key) && !force ) {
            return;
        }

        var info = Animations.Find(anim => anim.Key == key);
        _animator.Play(info.StateName);
        CurrentKey = key;
    }
}
