﻿using UnityEngine;
using System.Collections.Generic;

namespace Grigorov.Controllers {
    public class ControlledBehaviour<T> : MonoBehaviour where T : ControlledBehaviour<T> {
        public static List<T> AllObjects { get; private set; } = new List<T>();

        protected virtual void Awake() {
            AllObjects.Add(this as T);
        }

        protected virtual void OnDestroy() {
            AllObjects.Remove(this as T);
        }
    }
}