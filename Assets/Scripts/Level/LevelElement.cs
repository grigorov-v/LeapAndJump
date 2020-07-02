using System.Collections.Generic;
using UnityEngine;

using Grigorov.Extensions;
using Grigorov.LeapAndJump.Level.Gameplay;

namespace Grigorov.LeapAndJump.Level {
    public class LevelElement : BaseLevelElement {
        [SerializeField] List<Transform> _foodPoints = new List<Transform>();

        public void SpawnFood(Food foodPrefab) {
            if ( !foodPrefab ) {
                return;
            }
            
            var randomizePoints = _foodPoints.Randomize();
            var elemBounds = Bounds;
            foreach ( var point in randomizePoints ) {
                var canSpawn = Random.Range(0, 2) > 0;
                if ( !canSpawn ) {
                    continue;
                }

                var food = Instantiate(foodPrefab, transform);
                food.transform.SetParent(transform);                
                var pos = point.position;
                pos.y = elemBounds.center.y + elemBounds.extents.y + food.Bounds.extents.y;
                food.transform.position = pos;
            }
        }
    }
}