using System.Collections.Generic;
using System.Linq;
using Grigorov.Extensions;
using Grigorov.LeapAndJump.ResourcesContainers;
using Grigorov.Unity.Events;
using UnityEngine;

namespace Grigorov.LeapAndJump.Level {
	public readonly struct PlayerIntoBlockTriggerEnter {
		public readonly LevelBlock LevelBlock;

		public PlayerIntoBlockTriggerEnter(LevelBlock levelBlock) {
			LevelBlock = levelBlock;
		}
	}

	[RequireComponent(typeof(BoxCollider2D))]
	public class LevelBlock : MonoBehaviour {
		[SerializeField] Transform _beginPoint;
		[SerializeField] Transform _endPoint;
		[SerializeField] bool      _winBlock;

		[Space] 
		[SerializeField] List<SpriteRenderer> _backLayers = new List<SpriteRenderer>();

		LevelElementsGenerator _elementsGenerator;
		List<int>              _layers = new List<int>();

		public bool IsWinBlock => _winBlock;

		LevelElementsGenerator LevelElementsGenerator => this.GetComponent(ref _elementsGenerator);

		void OnTriggerEnter2D(Collider2D other) {
			if ( other.TryGetComponent(out Player player) || other.TryGetComponent(out PlayerStateHandler handler) ) {
				EventManager.Fire(new PlayerIntoBlockTriggerEnter(this));
			}
		}

		void OnValidate() {
			var boxTrigger = GetComponent<BoxCollider2D>();
			if ( !boxTrigger.isTrigger ) {
				boxTrigger.isTrigger = true;
			}
		}

		public void GenerateLevelElements(LevelElementsContainer elementsGroup) {
			if ( LevelElementsGenerator ) {
				LevelElementsGenerator.Generate(elementsGroup);
			}
		}

		public void SetPosition(LevelBlock lastBlock) {
			var position = Vector2.zero;
			if ( lastBlock ) {
				position = lastBlock._endPoint.position - _beginPoint.localPosition;
			}

			transform.position = position;
		}

		public void SetBackOrderLayer(int factor) {
			if ( _layers.Count == 0 ) {
				_layers = _backLayers.Select(bl => bl.sortingOrder).ToList();
			}

			for ( var i = 0; i < _backLayers.Count; i++ ) {
				_backLayers[i].sortingOrder = _layers[i] * factor;
			}
		}
	}
}