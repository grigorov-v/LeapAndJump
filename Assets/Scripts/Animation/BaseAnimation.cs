﻿using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

namespace Game.Animations {
    public abstract class BaseAnimation : MonoBehaviour {
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

        [ReorderableList] [SerializeField]
        protected List<AnimInfo> _animations = new List<AnimInfo>();

        public abstract void PlayAnimation(KeyAnim key);
    }
}