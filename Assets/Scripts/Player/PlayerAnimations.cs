using UnityEngine;
using Spine.Unity;

namespace Game.Animations.Player {
    public class PlayerAnimations : BaseAnimation {
        [SerializeField] SkeletonAnimation _skeletonAnimation = null;

        KeyAnim _curAnim = KeyAnim.None;

        public override void PlayAnimation(KeyAnim key) {
            if ( _curAnim == key ) {
                return;
            }

            var animName = _animations.Find(anim => anim.Key == key).StateName;
            _skeletonAnimation.loop = (key == KeyAnim.Walk) || (key == KeyAnim.SecondJump) || (key == KeyAnim.SlideInWall);
            _skeletonAnimation.AnimationName = animName;
            
            _curAnim = key;
        }
    }
}