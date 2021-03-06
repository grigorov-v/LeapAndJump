﻿using Grigorov.Extensions;
using Grigorov.LeapAndJump.Animations;
using Grigorov.LeapAndJump.UI;
using Grigorov.Unity.Controllers;
using Grigorov.Unity.Events;
using UnityEngine;

namespace Grigorov.LeapAndJump.Level {
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(PlayerAnimations))]
	public class Player : MonoBehaviour, IFixedUpdate, IUpdate {
		const float CheckAngle = 10f;

		[SerializeField] float   _speed         = 2.5f;
		[SerializeField] Vector2 _jumpForce     = new Vector2(60, 140);
		[SerializeField] Vector2 _slideVelosity = new Vector2(0.2f, -2);
		
		bool             _allowSecondJump;
		Collider2D       _floorTrigger;
		bool             _jump;
		PlayerAnimations _playerAnimations;

		Rigidbody2D _rb;
		Vector2     _wallNormal;
		Collider2D  _wallTrigger;

		Rigidbody2D      Rigidbody        => this.GetComponent(ref _rb);
		PlayerAnimations PlayerAnimations => this.GetComponent(ref _playerAnimations);

		bool CanJump        => (_floorTrigger || _wallTrigger) && _jump;
		bool CanSecondJump  => !_floorTrigger && !_wallTrigger && _allowSecondJump && _jump && Rigidbody.velocity.y <= 0;
		bool CanSlideInWall => !_floorTrigger && _wallTrigger && !_jump && Rigidbody.velocity.y < 0;

		bool CanMoveLeftOrRight {
			get {
				if ( !_floorTrigger && !_wallTrigger && Rigidbody.velocity == Vector2.zero && _jump ) //если застряли
				{
					return true;
				}

				return _floorTrigger && !_jump;
			}
		}

		UpdateController UpdateController => ControllersBox.Get<UpdateController>();

		public void Awake() {
			EventManager.Subscribe<TapZone_PointerDown>(this, OnPointerDown);
			UpdateController.AddUpdate(this as IUpdate);
			UpdateController.AddUpdate(this as IFixedUpdate);
		}

		public void OnDestroy() {
			EventManager.Unsubscribe<TapZone_PointerDown>(OnPointerDown);
			UpdateController.RemoveUpdate(this as IUpdate);
			UpdateController.RemoveUpdate(this as IFixedUpdate);
		}

		void OnCollisionEnter2D(Collision2D other) {
			if ( other.gameObject.TryGetComponent(out Food food) ) {
				return;
			}

			for ( var i = 0; i < other.contactCount; i++ ) {
				var normal = other.GetContact(i).normal;
				if ( IsWall(normal) && !_wallTrigger ) {
					_wallTrigger = other.collider;
					_wallNormal = normal;
					Rigidbody.velocity = Vector2.zero;
				}

				if ( IsFloor(normal) && !_floorTrigger ) {
					_floorTrigger = other.collider;
				}
			}

			_allowSecondJump = false;
		}

		void OnCollisionExit2D(Collision2D other) {
			if ( other.collider == _wallTrigger ) {
				_wallTrigger = null;
			}

			if ( other.collider == _floorTrigger ) {
				_floorTrigger = null;
			}

			if ( !_wallTrigger && !_floorTrigger ) {
				_allowSecondJump = true;
			}
		}

		void OnCollisionStay2D(Collision2D other) {
			if ( _jump ) {
				return;
			}

			for ( var i = 0; i < other.contactCount; i++ ) {
				var normal = other.GetContact(i).normal;
				if ( IsWall(normal) && !_wallTrigger ) {
					_wallTrigger = other.collider;
					_wallNormal = normal;
				}
			}
		}

		public void OnFixedUpdate() {
			if ( CanMoveLeftOrRight ) {
				MoveLeftOrRight();
				PlayerAnimations.PlayAnimation(KeyAnim.Walk);
			}

			if ( CanSlideInWall ) {
				SlideInWall();
				PlayerAnimations.PlayAnimation(KeyAnim.SlideInWall);
			}

			if ( CanJump ) {
				if ( _wallTrigger ) {
					SetMirrorScale();
				}

				Jump();
				_jump = false;
				PlayerAnimations.PlayAnimation(KeyAnim.Jump);
			}

			if ( CanSecondJump ) {
				Jump();
				_jump = false;
				_allowSecondJump = false;
				PlayerAnimations.PlayAnimation(KeyAnim.SecondJump);
			}

			if ( _floorTrigger && _wallTrigger ) {
				SetMirrorScale();
			}
		}

		public void OnUpdate() {
			if ( Input.GetKeyDown(KeyCode.Space) ) {
				_jump = true;
			}
		}

		void OnPointerDown(TapZone_PointerDown e) {
			_jump = true;
		}

		void Jump() {
			var jumpForce = _jumpForce;
			jumpForce.x = transform.localScale.x > 0 ? -Mathf.Abs(jumpForce.x) : Mathf.Abs(jumpForce.x);
			Rigidbody.velocity = Vector2.zero;
			Rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
		}

		void SlideInWall() {
			var slideVelocity = _slideVelosity;
			slideVelocity.x = transform.localScale.x > 0 ? -Mathf.Abs(slideVelocity.x) : Mathf.Abs(slideVelocity.x);
			Rigidbody.velocity = slideVelocity;
		}

		void MoveLeftOrRight() {
			if ( Rigidbody.velocity.y > 0 ) {
				return;
			}

			var dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
			Rigidbody.velocity = dir * _speed;
		}

		void SetMirrorScale() {
			var curScale = transform.localScale;
			curScale.x = Vector2.Angle(_wallNormal, Vector2.right) <= CheckAngle
				? -Mathf.Abs(curScale.x)
				: Mathf.Abs(curScale.x);
			transform.localScale = curScale;
		}

		bool IsWall(Vector2 normal) {
			var leftAngle = Vector2.Angle(normal, Vector2.left);
			var rightAngle = Vector2.Angle(normal, Vector2.right);
			return leftAngle <= CheckAngle || rightAngle <= CheckAngle;
		}

		bool IsFloor(Vector2 normal) {
			var angle = Vector2.Angle(normal, Vector2.up);
			return angle <= CheckAngle;
		}
	}
}