using UnityEngine;
using UnityEngine.UI;
using Grigorov.LeapAndJump.Animations;

using KeyAnim = Grigorov.LeapAndJump.Animations.BaseAnimation.KeyAnim;

namespace Grigorov.LeapAndJump.Player {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BaseAnimation))]
    public class PlayerControl : MonoBehaviour {
        const float CheckAngle = 10f;

        [SerializeField] float   _speed         = 2.5f;
        [SerializeField] Vector2 _jumpForce     = new Vector2(60, 140);
        [SerializeField] Vector2 _slideVelosity = new Vector2(0.2f, -2);

        Rigidbody2D   _rb               = null;
        BaseAnimation _animationControl = null;
        Collider2D    _wallTrigger      = null;
        Collider2D    _floorTrigger     = null;
        Vector2       _wallNormal       = Vector2.zero;  
        bool          _jumpInput        = false;
        bool          _allowSecondJump  = false;

        [Header("Debug")]
        public bool AutoPlay = false;
        public Text DebugText = null;
        float _jumpProbability = 30;

        bool CanJump {
            get {
                if ( !_floorTrigger && !_wallTrigger && (_rb.velocity == Vector2.zero) && _jumpInput ) {//если застряли
                    return true;
                }

                return (_floorTrigger || _wallTrigger) && _jumpInput;
            }
        }

        bool CanSecondJump {
            get => !_floorTrigger && !_wallTrigger && _allowSecondJump && _jumpInput && (_rb.velocity.y <= 0);
        }

        bool CanMoveLeftOrRight {
            get => _floorTrigger && !_jumpInput;
        }

        bool CanSlideInWall {
            get => !_floorTrigger && _wallTrigger && !_jumpInput && (_rb.velocity.y < 0);
        }

        void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _animationControl = GetComponent<BaseAnimation>();
        }

        void Update() {
            if ( !_jumpInput ) {
                _jumpInput = Input.GetKeyDown(KeyCode.Space);
                if ( AutoPlay ) {
                    var rand = Random.Range(0, 100);
                    _jumpInput = (rand <= _jumpProbability);
                }
            }

            if ( DebugText.gameObject.activeSelf && DebugText.gameObject.activeInHierarchy ) {
                var floorName = _floorTrigger ? _floorTrigger.name : "null";
                var wallName = _wallTrigger ? _wallTrigger.name : "null";
                DebugText.text = string.Format("floor: {0}\n\nwall: {1}\n\n", floorName, wallName);
                DebugText.text += string.Format("velocity: {0}\n", _rb.velocity);
                DebugText.text += string.Format("jump: {0}", _jumpInput);
            }
        }

        void FixedUpdate() {
            if ( CanMoveLeftOrRight ) {
                MoveLeftOrRight();
                _animationControl.PlayAnimation(KeyAnim.Walk);
            }
            
            if ( CanSlideInWall ) {
                SlideInWall();
                _animationControl.PlayAnimation(KeyAnim.SlideInWall);
            }

            if ( CanJump ) {
                if ( _wallTrigger ) {
                    SetMirrorScale();
                }
                Jump();
                _jumpInput = false;
                _animationControl.PlayAnimation(KeyAnim.Jump);
            }

            if ( CanSecondJump ) {  
                Jump(true);
                _jumpInput = false;
                _allowSecondJump = false;
                _animationControl.PlayAnimation(KeyAnim.SecondJump);
            }

            if ( _floorTrigger && _wallTrigger ) {
                SetMirrorScale();
            }
        }

        public void JumpInput() {
            _jumpInput = true;
        }

        void Jump(bool secondJump = false) {
            var jumpForce = _jumpForce;
            jumpForce.x = (transform.localScale.x > 0) ? -Mathf.Abs(jumpForce.x) : Mathf.Abs(jumpForce.x);
            _rb.velocity = Vector2.zero;
            _rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }

        void SlideInWall() {
            var slideVelosity = _slideVelosity;
            slideVelosity.x = (transform.localScale.x > 0) ? -Mathf.Abs(slideVelosity.x) : Mathf.Abs(slideVelosity.x);
            _rb.velocity = slideVelosity;
        }

        void MoveLeftOrRight() {
            if ( _rb.velocity.y > 0 ) {
                return;
            }
            var dir = (transform.localScale.x > 0) ? Vector2.left : Vector2.right;
            _rb.velocity = dir * _speed;
        }

        void SetMirrorScale() {
            var curScale = transform.localScale;
            curScale.x = (Vector2.Angle(_wallNormal, Vector2.right) <= CheckAngle) ? -Mathf.Abs(curScale.x) : Mathf.Abs(curScale.x);
            transform.localScale = curScale;
        }

        bool IsWall(Vector2 normal) {
            var leftAngle = Vector2.Angle(normal, Vector2.left);
            var rightAngle = Vector2.Angle(normal, Vector2.right);
            return (leftAngle <= CheckAngle) || (rightAngle <= CheckAngle);
        }

        bool IsFloor(Vector2 normal) {
            var angle = Vector2.Angle(normal, Vector2.up);
            return angle <= CheckAngle;
        }

        void OnCollisionEnter2D(Collision2D other) {
            for ( var i = 0; i < other.contactCount; i++ ) {        
                var normal = other.GetContact(i).normal;
                if ( IsWall(normal) && !_wallTrigger ) {
                    _wallTrigger = other.collider;
                    _wallNormal = normal;
                    _rb.velocity = Vector2.zero;
                    _allowSecondJump = false;
                }
                
                if ( IsFloor(normal) && !_floorTrigger ) {
                    _floorTrigger = other.collider;
                    _allowSecondJump = false;
                }
            }
        }

        void OnCollisionStay2D(Collision2D other) {
            if ( _jumpInput ) {
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
    }
}