using System;
using System.Collections.Generic;
using UnityEngine;

using Grigorov.Extensions;

namespace Grigorov.LeapAndJump.ResourcesContainers {
    public abstract class BaseResourcesContainer<T> : ScriptableObject {
        [SerializeField] List<T> _objects = new List<T>();

        List<T> _randomizeObjects = new List<T>();

        public List<T> RandomizeObjects {
            get => _objects.Randomize();
        }

        public T GetRandomObject(bool notRepetitive = true, Predicate<T> filter = null) {
            var objects = (filter != null) ? _objects.FindAll(filter) : _objects;

            if ( objects.Count == 0 ) {
                Debug.LogError("_levelElements.Count == 0");
                return default(T);
            }

            if ( !notRepetitive ) {
                return objects.GetRandomValue();
            }
            
            if ( _randomizeObjects.Count == 0 ) {
                _randomizeObjects = objects.Randomize();
            }
            var obj = _randomizeObjects.GetRandomValue();
            _randomizeObjects.Remove(obj);
            return obj;
        }
    }
}