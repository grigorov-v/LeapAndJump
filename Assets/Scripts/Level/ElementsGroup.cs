using System.Collections.Generic;
using UnityEngine;

using Grigorov.Extensions;

namespace Grigorov.LeapAndJump.Level {
    [CreateAssetMenu(menuName = "Create ElementsGroup", fileName = "ElementsGroup_Difficulty_{0}")]
    public class ElementsGroup : ScriptableObject {
        [SerializeField] List<LevelElement> _levelElements = new List<LevelElement>();

        List<LevelElement> _randomizeElements = new List<LevelElement>();

        public List<LevelElement> RandomizeElements {
            get => _levelElements.Randomize();
        }

        public LevelElement GetRandomLevelElement() {
            if ( _levelElements.Count == 0 ) {
                Debug.LogError("_levelElements.Count == 0");
                return null;
            }
            
            if ( _randomizeElements.Count == 0 ) {
                _randomizeElements = RandomizeElements;
            }

            var element = _randomizeElements.GetRandomElement();
            _randomizeElements.Remove(element);
            return element;
        }
    }
}