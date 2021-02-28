using System;
using UnityEngine;

using Grigorov.Extensions;
using Grigorov.LeapAndJump.UI;
using Grigorov.Unity.Events;

namespace Grigorov.LeapAndJump.Level {
	public enum PlayerState {
		Walk,
		Jump,
		SecondJump,
		SlideInWall,
		Flight
	}
	
	public class PlayerStateHandler : BaseStateHandler<PlayerState> {
		const float CheckAngle = 10f;

		[Space]
		[SerializeField] float   _moveSpeed     = 2.5f;
		[SerializeField] Vector2 _jumpForce     = new Vector2(60, 140);
		[SerializeField] Vector2 _slideVelocity = new Vector2(0.2f, -2);

		[Space] 
		[SerializeField] Transform _floorPoint;

		Rigidbody2D  _rb;
		LevelElement _floor;
		LevelElement _wall;
		Vector2      _lastWallNormal;
		
		PlayerStateView _stateView;
		bool            _jump;

		Rigidbody2D     Rigidbody       => this.GetComponent(ref _rb);
		PlayerStateView PlayerStateView => this.GetComponent(ref _stateView);

		protected void Awake() {
			AddTransition(PlayerState.Walk, PlayerState.Jump);
			AddTransition(PlayerState.Walk, PlayerState.Flight);

			AddTransition(PlayerState.Jump, PlayerState.SecondJump);
			AddTransition(PlayerState.Jump, PlayerState.SlideInWall);
			AddTransition(PlayerState.Jump, PlayerState.Walk);
			AddTransition(PlayerState.Jump, PlayerState.Flight);

			AddTransition(PlayerState.SecondJump, PlayerState.SlideInWall);
			AddTransition(PlayerState.SecondJump, PlayerState.Walk);
			AddTransition(PlayerState.SecondJump, PlayerState.Flight);

			AddTransition(PlayerState.SlideInWall, PlayerState.Walk);
			AddTransition(PlayerState.SlideInWall, PlayerState.Jump);
			AddTransition(PlayerState.SlideInWall, PlayerState.Flight);
			
			AddTransition(PlayerState.Flight, PlayerState.Walk);
			AddTransition(PlayerState.Flight, PlayerState.SlideInWall);
			AddTransition(PlayerState.Flight, PlayerState.SecondJump);
			
			EventManager.Subscribe<TapZone_PointerDown>(this, OnTapZonePointerDown);
		}

		protected void Start() {
			SetState(PlayerState.Walk);
			PlayerStateView.Play(PlayerState.Walk);
		}

		void FixedUpdate() {
			if ( _jump ) {
				Jump();
				_jump = false;
				return;
			}

			switch (State) {
				case PlayerState.Walk:
					Walk();
					return;
				case PlayerState.SlideInWall:
					SlideInWall();
					return;
			}
		}

		protected void OnDestroy() {
			EventManager.Unsubscribe<TapZone_PointerDown>(OnTapZonePointerDown);
		}

		bool IsWall(Vector2 normal) {
			var leftAngle = Vector2.Angle(normal, Vector2.left);
			var rightAngle = Vector2.Angle(normal, Vector2.right);
			return leftAngle <= CheckAngle || rightAngle <= CheckAngle;
		}
		
		bool IsWall(Collision2D collision2D, out ContactPoint2D contactPoint) {
			contactPoint = collision2D.GetContact(0);
			for ( var i = 0; i < collision2D.contactCount; i++ ) {
				var normal = collision2D.GetContact(i).normal;
				if ( !IsWall(normal) ) {
					continue;
				}

				contactPoint = collision2D.GetContact(i);
				return true;
			}
			return false;
		}

		bool IsFloor(Vector2 normal) {
			var angle = Vector2.Angle(normal, Vector2.up);
			return angle <= CheckAngle;
		}
		
		bool IsFloor(Collision2D collision2D, out ContactPoint2D contactPoint) {
			contactPoint = collision2D.GetContact(0);
			for ( var i = 0; i < collision2D.contactCount; i++ ) {
				var normal = collision2D.GetContact(i).normal;
				if ( !IsFloor(normal) ) {
					continue;
				}

				contactPoint = collision2D.GetContact(i);
				return true;
			}
			return false;
		}

		void Walk() {
			if ( Rigidbody.velocity.y > 0 ) {
				return;
			}

			var dir = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
			var velocity = Rigidbody.velocity;
			var newVelocity = dir * _moveSpeed;
			velocity.x = newVelocity.x;
			Rigidbody.velocity = velocity;
		}

		void SlideInWall() {
			var slideVelocity = _slideVelocity;
			slideVelocity.x = transform.localScale.x > 0 ? -Mathf.Abs(slideVelocity.x) : Mathf.Abs(slideVelocity.x);
			Rigidbody.velocity = slideVelocity;
		}

		void MirrorScale(Vector2 collisionNormal) {
			var curScale = transform.localScale;
			curScale.x = Vector2.Angle(collisionNormal, Vector2.right) <= CheckAngle
				? -Mathf.Abs(curScale.x)
				: Mathf.Abs(curScale.x);
			transform.localScale = curScale;
		}

		void Jump() {
			if ( !SetState(PlayerState.Jump) ) {
				if ( !SetState(PlayerState.SecondJump) ) {
					return;
				}
			}

			var jumpForce = _jumpForce;
			jumpForce.x = transform.localScale.x > 0 ? -Mathf.Abs(jumpForce.x) : Mathf.Abs(jumpForce.x);
			Rigidbody.velocity = Vector2.zero;
			Rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
		}

		void CheckCollision(Collision2D collision2D) {
			if ( collision2D.collider.TryGetComponent(out Food food) ) {
				return;
			}
			
			if ( IsWall(collision2D, out ContactPoint2D contactPoint) ) {
				_wall = collision2D.collider.GetComponentInParent<LevelElement>();
				_lastWallNormal = contactPoint.normal;
				SetState(PlayerState.SlideInWall);
				if ( State == PlayerState.Walk ) {
					MirrorScale(_lastWallNormal);
				}
				return;
			} 
			
			if ( IsFloor(collision2D, out contactPoint) ) {
				if ( _floorPoint.position.y < contactPoint.point.y ) {
					return;
				}
				_floor = collision2D.collider.GetComponentInParent<LevelElement>();;
				SetState(PlayerState.Walk);
			}
		}

		protected override void OnChangedState(PlayerState from, PlayerState to) {
			PlayerStateView.Play(to);

			if ( (from == PlayerState.SlideInWall) && (to == PlayerState.Walk) ) {
				MirrorScale(_lastWallNormal);
			}
			
			if ( (from == PlayerState.SlideInWall) && (to == PlayerState.Jump) ) {
				MirrorScale(_lastWallNormal);
			}
		}
		
		void OnTapZonePointerDown(TapZone_PointerDown e) {
			_jump = true;
		}

		void OnCollisionEnter2D(Collision2D other) {
			if ( _jump ) {
				return;
			}

			CheckCollision(other);
		}

		void OnCollisionStay2D(Collision2D other) {
			if ( _floor || _wall ) {
				return;
			}

			if ( State != PlayerState.Flight ) {
				return;
			}
			
			CheckCollision(other);
		}

		void OnCollisionExit2D(Collision2D other) {
			var levelElement = other.collider.GetComponentInParent<LevelElement>();;
			if ( levelElement == _floor ) {
				_floor = null;
			}
			
			if ( levelElement == _wall ) {
				_wall = null;
			}
			
			if ( !_floor && !_wall ) {
				SetState(PlayerState.Flight);
			}
		}
	}
}
