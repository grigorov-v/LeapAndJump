﻿using UnityEngine;
using Spine.Unity;

using Grigorov.Extensions;
using Grigorov.LeapAndJump.Level;

namespace Grigorov.LeapAndJump.Animations {
    public class PlayerAnimations : BaseAnimation {
        [SerializeField] SkeletonAnimation _skeletonAnimation = null;

        KeyAnim _curAnim = KeyAnim.None;
        Player  _player  = null;

        public override void PlayAnimation(KeyAnim key) {
            if ( _curAnim == key ) {
                return;
            }

            var animName = _animations.Find(anim => anim.Key == key).StateName;
            _skeletonAnimation.loop = (key == KeyAnim.Walk) || (key == KeyAnim.SecondJump) || 
                                      (key == KeyAnim.SlideInWall) || (key == KeyAnim.Idle);
                                      
            _skeletonAnimation.AnimationName = animName;
            _curAnim = key;
        }
    }
}