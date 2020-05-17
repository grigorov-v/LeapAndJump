using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Grigorov.Extensions;

using NaughtyAttributes;

namespace Grigorov.LeapAndJump.Level {
    [CreateAssetMenu(menuName = "Create LevelsBalance", fileName = "World_{0}_LevelsBalance")]
    public class LevelsBalance : ScriptableObject {
        [System.Serializable]
        struct Level {
            public List<ElementsGroup> ElementsGroups;
        }

        [SerializeField][ReorderableList] List<Level> _levels = new List<Level>();

        List<ElementsGroup> _randomizeElementsGroups = new List<ElementsGroup>();

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
            
            if ( _randomizeElementsGroups.Count == 0 ) {
                _randomizeElementsGroups = elementsGroups.Randomize();
            }

            var group = _randomizeElementsGroups.GetRandomElement();
            _randomizeElementsGroups.Remove(group);
            return group;
        }
    }
}
