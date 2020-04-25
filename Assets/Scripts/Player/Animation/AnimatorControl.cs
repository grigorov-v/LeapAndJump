using UnityEngine;

public class AnimatorControl : BaseAnimationControl {
    Animator _animator   = null;

    private void Awake() {
        _animator = GetComponentInChildren<Animator>();
    }

    public override void PlayAnimation(KeyAnim key) {
        var info = Animations.Find(anim => anim.Key == key);
        _animator.Play(info.StateName);
    }
}
