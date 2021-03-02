using System.Linq;
using Grigorov.LeapAndJump.Level;
using UnityEngine;

namespace Grigorov.LeapAndJump.Utils {
	public class CollidersGenerator : MonoBehaviour {
		[NaughtyAttributes.Button]
		void Generate() {
			var element = gameObject.AddComponent<LevelElement>();
			element.Centring();
			
			var bounds = element.Bounds;
			var edgeRadius =  0.02f;
			var collider = gameObject.AddComponent<BoxCollider2D>();
			
			collider.edgeRadius = edgeRadius;
			var size = bounds.size;
			size.x -= edgeRadius * 2;
			size.y -= edgeRadius * 2;
			collider.size = size;
			
			DestroyImmediate(element);
		}

		[NaughtyAttributes.Button]
		void RemoveAllCollider() {
			var colliders = GetComponents<Collider2D>().ToList();
			colliders.ForEach(DestroyImmediate);
		}
	}
}
