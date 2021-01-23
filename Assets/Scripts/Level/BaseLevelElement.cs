using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grigorov.LeapAndJump.Level {
	public abstract class BaseLevelElement : MonoBehaviour {
		Bounds _bounds;
		Vector3 _lastPosition = Vector3.zero;
		readonly List<SpriteRenderer> _spritesRenderers = new List<SpriteRenderer>();

		public Bounds Bounds {
			get {
				if ( !IsRecalculateBounds ) {
					return _bounds;
				}

				if ( SpritesRenderers.Count == 1 ) {
					_bounds = SpritesRenderers.First().bounds;
				}
				else {
					var size = Vector2.zero;
					size.x = TopRightPoint.x - BottomLeftPoint.x;
					size.y = TopRightPoint.y - BottomLeftPoint.y;
					_bounds = new Bounds(Center, size);
				}

				_lastPosition = transform.position;
				return _bounds;
			}
		}

		List<Bounds> ListBounds {
			get {
				var listBounds = new List<Bounds>();
				SpritesRenderers.ForEach(sr => listBounds.Add(sr.bounds));
				return listBounds;
			}
		}

		Vector2 TopRightPoint {
			get {
				var maxX = 0f;
				var maxY = 0f;
				for ( var i = 0; i < ListBounds.Count; i++ ) {
					var bounds = ListBounds[i];
					var x = bounds.center.x + bounds.extents.x;
					var y = bounds.center.y + bounds.extents.y;
					if ( i == 0 ) {
						maxX = x;
						maxY = y;
						continue;
					}

					maxX = x > maxX ? x : maxX;
					maxY = y > maxY ? y : maxY;
				}

				return new Vector2(maxX, maxY);
			}
		}

		Vector2 BottomLeftPoint {
			get {
				var minX = 0f;
				var minY = 0f;
				for ( var i = 0; i < ListBounds.Count; i++ ) {
					var bounds = ListBounds[i];
					var x = bounds.center.x - bounds.extents.x;
					var y = bounds.center.y - bounds.extents.y;
					if ( i == 0 ) {
						minX = x;
						minY = y;
						continue;
					}

					minX = x < minX ? x : minX;
					minY = y < minY ? y : minY;
				}

				return new Vector2(minX, minY);
			}
		}

		Vector2 Center {
			get {
				var x = (BottomLeftPoint.x + TopRightPoint.x) / 2;
				var y = (BottomLeftPoint.y + TopRightPoint.y) / 2;
				return new Vector2(x, y);
			}
		}

		List<SpriteRenderer> SpritesRenderers {
			get {
				if ( _spritesRenderers.Count == 0 ) {
					GetComponentsInChildren(false, _spritesRenderers);
				}

				return _spritesRenderers;
			}
		}

		bool IsRecalculateBounds => _bounds.size == Vector3.zero || transform.position != _lastPosition;

		void OnDrawGizmos() {
			_bounds = new Bounds();
			SpritesRenderers.Clear();
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(Bounds.center, Bounds.size);
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(Bounds.center, 0.05f);
		}
	}
}