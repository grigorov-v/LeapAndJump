using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

namespace Grigorov.LeapAndJump.Level {
    [CreateAssetMenu(menuName = "Create LevelsBalance", fileName = "World_{0}_LevelsBalance")]
    public class LevelsBalance : ScriptableObject {
        [System.Serializable]
        struct Level {
            public List<ElementsGroup> ElementsGroups;
        }

        [SerializeField][ReorderableList] List<Level> _levels = new List<Level>();

        public ElementsGroup GetRandomElementsGroup(int levelIndex) {
            if ( levelIndex > _levels.Count - 1 ) {
                return null;
            }
            var level = _levels[levelIndex];
            return GetRandomElementsGroup(level);
        }

        ElementsGroup GetRandomElementsGroup(Level level) {
            var elementsGroups = level.ElementsGroups;
            if ( elementsGroups.Count == 0 ) {
                return null;
            }
            var randIndex = Random.Range(0, elementsGroups.Count);
            return elementsGroups[randIndex];
        }
    }
}
