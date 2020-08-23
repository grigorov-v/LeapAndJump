using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Grigorov.Extensions;

using NaughtyAttributes;

namespace Grigorov.LeapAndJump.Level {
    public class LevelElement : BaseLevelElement {
        [SerializeField] List<Transform> _foodPoints   = new List<Transform>();
        [SerializeField] bool            _mirrorXScale = false;

        public void TryMirror() {
            if ( !_mirrorXScale ) {
                return;
            }
            var scale = transform.localScale;
            scale.x = (Random.Range(0, 2) > 0) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

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


        [Button]
        void TryFindFoodPoints() {
            _foodPoints = GetComponentsInChildren<Transform>().Where(point => point.gameObject.name == "FoodPoint").ToList();
        }
    }
}