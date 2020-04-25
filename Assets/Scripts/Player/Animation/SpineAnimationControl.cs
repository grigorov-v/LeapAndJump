using UnityEngine;
using Spine.Unity;

public class SpineAnimationControl : BaseAnimationControl {
    SkeletonAnimation _skeletonAnimation = null;

    void Awake() {
        _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
    }

    public override void PlayAnimation(KeyAnim key) {
        var animName = Animations.Find(anim => anim.Key == key).StateName;
        _skeletonAnimation.AnimationName = animName;
        _skeletonAnimation.loop = (key == KeyAnim.Walk) || (key == KeyAnim.SecondJump) || (key == KeyAnim.SlideInWall);
    }
}