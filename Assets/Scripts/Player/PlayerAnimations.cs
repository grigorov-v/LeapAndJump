using UnityEngine;
using Spine.Unity;

namespace Game.Animations.Player {
    public class PlayerAnimations : BaseAnimation {
        [SerializeField] SkeletonAnimation _skeletonAnimation = null;

        public override void PlayAnimation(KeyAnim key) {
            var animName = _animations.Find(anim => anim.Key == key).StateName;
            _skeletonAnimation.AnimationName = animName;
            _skeletonAnimation.loop = (key == KeyAnim.Walk) || (key == KeyAnim.SecondJump) || (key == KeyAnim.SlideInWall);  
        }
    }
}