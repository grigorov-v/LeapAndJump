using System.Collections.Generic;
using System.Linq;
using Grigorov.Extensions;
using Grigorov.LeapAndJump.ResourcesContainers;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Grigorov.LeapAndJump.Level {
	public class LevelElement : BaseLevelElement {
		[SerializeField] List<Transform> _foodPoints = new List<Transform>();
		[SerializeField] bool            _mirrorXScale;

		public void TryMirror() {
			if ( !_mirrorXScale ) {
				return;
			}

			var scale = transform.localScale;
			scale.x = Random.Range(0, 2) > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
			transform.localScale = scale;
		}

		public int SpawnFoods(FoodsContainer foods) {
			if ( !foods ) {
				return 0;
			}

			var randomizePoints = _foodPoints.Randomize();
			var elemBounds = Bounds;
			var count = 0;
			foreach ( var point in randomizePoints ) {
				var canSpawn = Random.Range(0, 2) > 0;
				if ( !canSpawn ) {
					continue;
				}

				var foodPrefab = foods.GetRandomObject(false);
				if ( !foodPrefab ) {
					continue;
				}

				var food = Instantiate(foodPrefab, transform);
				var foodTransform = food.transform;
				foodTransform.rotation = Quaternion.identity;
				var pos = point.position;
				pos.y = elemBounds.center.y + elemBounds.extents.y + food.Bounds.extents.y;

				var posDelta = pos - foodTransform.position;
				foodTransform.position = pos;
				count++;
			}

			return count;
		}


		[Button]
		void TryFindFoodPoints() {
			_foodPoints = GetComponentsInChildren<Transform>().Where(point => point.gameObject.name == "FoodPoint")
				.ToList();

#if UNITY_EDITOR
			EditorUtility.SetDirty(gameObject);
#endif
		}

		[Button]
		public void Centring() {
			var spriteRenderers = new List<SpriteRenderer>();
			GetComponentsInChildren<SpriteRenderer>(spriteRenderers);

			var parents = new List<Transform>();
			foreach ( var sr in spriteRenderers) {
				parents.Add(sr.transform.parent);
			}
			
			foreach ( var sr in spriteRenderers) {
				sr.transform.SetParent(null);
			}

			var targetPos = Bounds.center;
			transform.position = targetPos;

			for ( var i = 0; i < spriteRenderers.Count; i++ ) {
				spriteRenderers[i].transform.SetParent(parents[i]);
			}
			
#if UNITY_EDITOR
			EditorUtility.SetDirty(gameObject);
#endif
		}
	}
}