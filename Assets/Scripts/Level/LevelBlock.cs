using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Grigorov.Unity.Events;
using Grigorov.Extensions;

using Grigorov.LeapAndJump.ResourcesContainers;

namespace Grigorov.LeapAndJump.Level
{
	public struct PlayerIntoBlockTriggerEnter
	{
		public LevelBlock LevelBlock { get; private set; }
		public PlayerController     Player     { get; private set; }

		public PlayerIntoBlockTriggerEnter(LevelBlock levelBlock, PlayerController player)
		{
			LevelBlock = levelBlock;
			Player = player;
		}
	}

	[RequireComponent(typeof(BoxCollider2D))]
	public class LevelBlock : MonoBehaviour
	{
		[SerializeField] Transform _beginPoint = null;
		[SerializeField] Transform _endPoint   = null;
		[SerializeField] bool      _winBlock   = false;

		[Space]
		[SerializeField] List<SpriteRenderer> _backLayers = new List<SpriteRenderer>();

		LevelElementsGenerator _elementsGenerator = null;
		List<int>              _layers            = new List<int>();

		public bool IsWinBlock => _winBlock;

		LevelElementsGenerator LevelElementsGenerator => this.GetComponent(ref _elementsGenerator);

		void OnValidate()
		{
			var boxTrigger = GetComponent<BoxCollider2D>();
			if (!boxTrigger.isTrigger)
			{
				boxTrigger.isTrigger = true;
			}
		}

		public void GenerateLevelElements(LevelElementsContainer elementsGroup)
		{
			if (LevelElementsGenerator)
			{
				LevelElementsGenerator.Generate(elementsGroup);
			}
		}

		public void SetPosition(LevelBlock lastBlock)
		{
			var position = Vector2.zero;
			if (lastBlock)
			{
				position = lastBlock._endPoint.position - _beginPoint.localPosition;
			}
			transform.position = position;
		}

		public void SetBackOrderLayer(int factor)
		{
			if (_layers.Count == 0)
			{
				_layers = _backLayers.Select(bl => bl.sortingOrder).ToList();
			}

			for (var i = 0; i < _backLayers.Count; i++)
			{
				_backLayers[i].sortingOrder = _layers[i] * factor;
			}
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent(out PlayerController player))
			{
				EventManager.Fire(new PlayerIntoBlockTriggerEnter(this, player));
			}
		}
	}
}