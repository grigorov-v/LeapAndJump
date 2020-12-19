﻿using System;
using UnityEngine;

using Grigorov.Unity.Events;
using Grigorov.Extensions;

using Grigorov.LeapAndJump.Events;

using DG.Tweening;

namespace Grigorov.LeapAndJump.Level
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class Food : BaseLevelElement
	{
		[SerializeField] float _tweenDuration = 0.5f;

		bool           _isCollecting = false;
		Tween          _tweenCollect = null;
		Rigidbody2D    _rb           = null;
		SpriteRenderer _renderer     = null;

		public SpriteRenderer Renderer => this.GetComponentInChildren(ref _renderer);

		Rigidbody2D Rigidbody => this.GetComponent(ref _rb);

		void StartCollect()
		{
			ResetTween();

			Rigidbody.Sleep();
			Rigidbody.isKinematic = true;

			var colliders = new Collider2D[Rigidbody.attachedColliderCount];
			Rigidbody.GetAttachedColliders(colliders);
			Array.ForEach(colliders, coll => coll.enabled = false);

			_tweenCollect = transform.DOScale(Vector2.zero, _tweenDuration);
			_tweenCollect.onComplete += () =>
			{
				if (gameObject)
				{
					Collect();
					Destroy(gameObject);
				}
			};

			_isCollecting = true;
		}

		void Collect()
		{
			if (!_isCollecting)
			{
				return;
			}
			EventManager.Fire(new FoodCollectEvent());
		}

		void ResetTween()
		{
			if (_tweenCollect == null)
			{
				return;
			}
			_tweenCollect.Kill();
			_tweenCollect = null;
		}

		void OnTriggerEnter2D(Collider2D other)
		{
			var player = other.GetComponent<PlayerController>();
			if (player)
			{
				StartCollect();
			}
		}

		void OnDestroy()
		{
			ResetTween();
		}
	}
}