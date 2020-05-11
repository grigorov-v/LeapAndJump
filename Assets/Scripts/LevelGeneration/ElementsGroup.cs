using System.Collections.Generic;
using UnityEngine;

namespace Grigorov.LeapAndJump.Level {
    [CreateAssetMenu(menuName = "Create ElementsGroup", fileName = "ElementsGroup_Difficulty_{0}")]
    public class ElementsGroup : ScriptableObject {
        [SerializeField] List<LevelElement> _levelElements  = new List<LevelElement>();

        public LevelElement GetRandomLevelElement() {
            if ( _levelElements.Count == 0 ) {
                Debug.LogError("_levelElements.Count == 0");
                return null;
            }
            
            var count = _levelElements.Count;
            var randIndex = Random.Range(0, count);
            return _levelElements[randIndex];
        }
    }
}