using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Grigorov.Extensions;
using Grigorov.LeapAndJump.ResourcesContainers;

using NaughtyAttributes;

namespace Grigorov.LeapAndJump.Level
{
	public class LevelElement : BaseLevelElement
	{
		[SerializeField] List<Transform> _foodPoints   = new List<Transform>();
		[SerializeField] bool            _mirrorXScale = false;

		public void TryMirror()
		{
			if (!_mirrorXScale)
			{
				return;
			}
			var scale = transform.localScale;
			scale.x = (Random.Range(0, 2) > 0) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
			transform.localScale = scale;
		}

		public int SpawnFoods(FoodsContainer foods)
		{
			if (!foods)
			{
				return 0;
			}

			var randomizePoints = _foodPoints.Randomize();
			var elemBounds = Bounds;
			var count = 0;
			foreach (var point in randomizePoints)
			{
				var canSpawn = Random.Range(0, 2) > 0;
				if (!canSpawn)
				{
					continue;
				}

				var foodPrefab = foods.GetRandomObject(notRepetitive: false);
				if (!foodPrefab)
				{
					continue;
				}

				var food = Instantiate(foodPrefab, transform);
				food.transform.SetParent(transform);
				var pos = point.position;
				pos.y = elemBounds.center.y + elemBounds.extents.y + food.Bounds.extents.y;
				food.transform.position = pos;
				count++;
			}

			return count;
		}


		[Button]
		void TryFindFoodPoints()
		{
			_foodPoints = GetComponentsInChildren<Transform>().Where(point => point.gameObject.name == "FoodPoint").ToList();
		}
	}
}