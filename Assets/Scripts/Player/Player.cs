﻿using UnityEngine;
using System.Collections.Generic;

using Grigorov.Extensions;
using Grigorov.Controllers;

using Grigorov.LeapAndJump.Animations;
using Grigorov.LeapAndJump.Effects;

namespace Grigorov.LeapAndJump.Level
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(PlayerAnimations))]
	public class Player : ControlledBehaviour<Player>
	{
		const float CheckAngle = 10f;

		[SerializeField] float   _speed         = 2.5f;
		[SerializeField] Vector2 _jumpForce     = new Vector2(60, 140);
		[SerializeField] Vector2 _slideVelosity = new Vector2(0.2f, -2);

		[Space]
		[SerializeField] List<Trail> _jumpTrails = new List<Trail>();

		Rigidbody2D      _rb               = null;
		PlayerAnimations _playerAnimations = null;
		Collider2D       _wallTrigger      = null;
		Collider2D       _floorTrigger     = null;
		Vector2          _wallNormal       = Vector2.zero;
		bool             _jump             = false;
		bool             _allowSecondJump  = false;

		Rigidbody2D      Rigidbody        => this.GetComponent(ref _rb);
		PlayerAnimations PlayerAnimations => this.GetComponent(ref _playerAnimations);

		bool CanJump        => (_floorTrigger || _wallTrigger) && _jump;
		bool CanSecondJump  => !_floorTrigger && !_wallTrigger && _allowSecondJump && _jump && (Rigidbody.velocity.y <= 0);
		bool CanSlideInWall => !_floorTrigger && _wallTrigger && !_jump && (Rigidbody.velocity.y < 0);

		bool CanMoveLeftOrRight
		{
			get
			{
				if (!_floorTrigger && !_wallTrigger && (Rigidbody.velocity == Vector2.zero) && _jump)
				{//если застряли
					return true;
				}
				return _floorTrigger && !_jump;
			}
		}

		public void JumpInput()
		{
			_jump = true;
		}

		public void OnUpdate()
		{
			if (CanMoveLeftOrRight)
			{
				MoveLeftOrRight();
				PlayerAnimations.PlayAnimation(KeyAnim.Walk);
			}

			if (CanSlideInWall)
			{
				SlideInWall();
				PlayerAnimations.PlayAnimation(KeyAnim.SlideInWall);
			}

			if (CanJump)
			{
				if (_wallTrigger)
				{
					SetMirrorScale();
				}
				Jump();
				_jump = false;
				PlayerAnimations.PlayAnimation(KeyAnim.Jump);
			}

			if (CanSecondJump)
			{
				Jump(true);
				_jump = false;
				_allowSecondJump = false;
				PlayerAnimations.PlayAnimation(KeyAnim.SecondJump);
			}

			if (_floorTrigger && _wallTrigger)
			{
				SetMirrorScale();
			}
		}

		void PlayJumpTrails()
		{
			_jumpTrails.ForEach(trail => trail.Play());
		}

		void StopJumpTrails()
		{
			_jumpTrails.ForEach(trail => trail.Stop());
		}

		void Jump(bool secondJump = false)
		{
			var jumpForce = _jumpForce;
			jumpForce.x = (transform.localScale.x > 0) ? -Mathf.Abs(jumpForce.x) : Mathf.Abs(jumpForce.x);
			Rigidbody.velocity = Vector2.zero;
			Rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
			PlayJumpTrails();
		}

		void SlideInWall()
		{
			var slideVelosity = _slideVelosity;
			slideVelosity.x = (transform.localScale.x > 0) ? -Mathf.Abs(slideVelosity.x) : Mathf.Abs(slideVelosity.x);
			Rigidbody.velocity = slideVelosity;
			StopJumpTrails();
		}

		void MoveLeftOrRight()
		{
			if (Rigidbody.velocity.y > 0)
			{
				return;
			}
			var dir = (transform.localScale.x > 0) ? Vector2.left : Vector2.right;
			Rigidbody.velocity = dir * _speed;
			StopJumpTrails();
		}

		void SetMirrorScale()
		{
			var curScale = transform.localScale;
			curScale.x = (Vector2.Angle(_wallNormal, Vector2.right) <= CheckAngle) ? -Mathf.Abs(curScale.x) : Mathf.Abs(curScale.x);
			transform.localScale = curScale;
		}

		bool IsWall(Vector2 normal)
		{
			var leftAngle = Vector2.Angle(normal, Vector2.left);
			var rightAngle = Vector2.Angle(normal, Vector2.right);
			return (leftAngle <= CheckAngle) || (rightAngle <= CheckAngle);
		}

		bool IsFloor(Vector2 normal)
		{
			var angle = Vector2.Angle(normal, Vector2.up);
			return angle <= CheckAngle;
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.TryGetComponent<Food>(out Food food))
			{
				return;
			}

			for (var i = 0; i < other.contactCount; i++)
			{
				var normal = other.GetContact(i).normal;
				if (IsWall(normal) && !_wallTrigger)
				{
					_wallTrigger = other.collider;
					_wallNormal = normal;
					Rigidbody.velocity = Vector2.zero;
				}

				if (IsFloor(normal) && !_floorTrigger)
				{
					_floorTrigger = other.collider;
				}
			}

			_allowSecondJump = false;
		}

		void OnCollisionStay2D(Collision2D other)
		{
			if (_jump)
			{
				return;
			}

			for (var i = 0; i < other.contactCount; i++)
			{
				var normal = other.GetContact(i).normal;
				if (IsWall(normal) && !_wallTrigger)
				{
					_wallTrigger = other.collider;
					_wallNormal = normal;
				}
			}
		}

		void OnCollisionExit2D(Collision2D other)
		{
			if (other.collider == _wallTrigger)
			{
				_wallTrigger = null;
			}

			if (other.collider == _floorTrigger)
			{
				_floorTrigger = null;
			}

			if (!_wallTrigger && !_floorTrigger)
			{
				_allowSecondJump = true;
			}
		}
	}
}