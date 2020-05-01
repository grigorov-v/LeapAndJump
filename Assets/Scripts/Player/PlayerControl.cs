using UnityEngine;
using Game.Level;

using KeyAnim = AnimatorControl.KeyAnim;

namespace Game.Player {

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BaseAnimationControl))]
    public class PlayerControl : MonoBehaviour {
        const float CheckAngle = 10f;

        [SerializeField] float   _speed         = 2.5f;
        [SerializeField] Vector2 _jumpForce     = new Vector2(60, 140);
        [SerializeField] Vector2 _slideVelosity = new Vector2(0.2f, -2);
        [SerializeField] Bounds  _localBounds   = new Bounds();

        Rigidbody2D          _rb               = null;
        BaseAnimationControl _animationControl = null;
        [SerializeField] Collider2D           _wallTrigger      = null;
        [SerializeField] Collider2D           _floorTrigger     = null;
        Vector2              _wallNormal       = Vector2.zero;  
        bool                 _jumpTrigger      = false;
        bool                 _allowSecondJump  = false;
        bool                 _jumpProcess      = false;

        [Header("Debug")]
        public bool AutoPlay = false;
        float _jumpProbability = 30;

        bool CanMoveLeftOrRight {
            get {
                return _floorTrigger;
            }
        }

        bool CanJump {
            get {
                return _floorTrigger || _wallTrigger;
            }
        }

        bool CanSecondJump {
            get {
                return !_floorTrigger && !_wallTrigger;
            }
        }

        bool CanSlideInWall {
            get {
                return !_floorTrigger && _wallTrigger && !_jumpProcess;
            }
        }

        Bounds Bounds {
            get {
                var worldBounds = _localBounds;
                worldBounds.center = transform.TransformPoint(_localBounds.center);
                return worldBounds;
            }
        }

        void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _animationControl = GetComponent<BaseAnimationControl>();
        }

        void Update() {
            if ( !_jumpTrigger ) {
                _jumpTrigger = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);

                if ( AutoPlay ) {
                    var rand = Random.Range(0, 100);
                    _jumpTrigger = (rand <= _jumpProbability);
                }
            }
        }

        void FixedUpdate() {
            var isJump = false;
            if ( CanJump && _jumpTrigger ) {
                Jump();
                _jumpTrigger = false;
                _jumpProcess = true;
                _allowSecondJump = true;
                isJump = true;
            }

            if ( CanSecondJump && _allowSecondJump && _jumpTrigger ) {  
                Jump(true);
                _jumpTrigger = false;
                _allowSecondJump = false;
                isJump = true;
            }

            if ( isJump ) {
                _wallTrigger = null;
                _floorTrigger = null;
            }

            if ( CanMoveLeftOrRight ) {
                MoveLeftOrRight();
            } 
            
            if ( CanSlideInWall ) {
                SlideInWall();
            }
        }

        void Jump(bool secondJump = false) {
            var jumpForce = _jumpForce;
            jumpForce.x = (transform.localScale.x > 0) ? -Mathf.Abs(jumpForce.x) : Mathf.Abs(jumpForce.x);
            if ( _wallTrigger && !_floorTrigger ) {
                ChangeMirrorScale();
                jumpForce.x *= -1;
            }

            _rb.velocity = Vector2.zero;
            _rb.AddForce(jumpForce, ForceMode2D.Impulse);

            var keyAnim = !secondJump ? KeyAnim.Jump : KeyAnim.SecondJump;
            _animationControl.PlayAnimation(keyAnim);
        }

        void SlideInWall() {
            var slideVelosity = _slideVelosity;
            slideVelosity.x = (transform.localScale.x > 0) ? -Mathf.Abs(slideVelosity.x) : Mathf.Abs(slideVelosity.x);
            _rb.velocity = slideVelosity;
            _animationControl.PlayAnimation(KeyAnim.SlideInWall);
        }

        void MoveLeftOrRight() {
            ChangeMirrorScale();
            var dir = (transform.localScale.x > 0) ? Vector2.left : Vector2.right;
            var movePos = (Vector2)transform.position + (dir * _speed * Time.fixedDeltaTime);
            _rb.MovePosition(movePos);
            _animationControl.PlayAnimation(KeyAnim.Walk);
        }

        void ChangeMirrorScale() {
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

        void CheckWall(Collider2D collider, Vector2 normal) {
            if ( IsWall(normal) && !_wallTrigger ) {
                _wallTrigger = collider;
                _wallNormal = normal;
                _allowSecondJump = true;
            }
        }

        void CheckFloor(Collider2D collider, Vector2 normal) {
            if ( IsFloor(normal) && !_floorTrigger ) {
                var levelElement = collider.GetComponent<LevelElement>();
                if ( levelElement && levelElement.Floor ) {
                    if ( (Bounds.center.y - Bounds.extents.y) < (levelElement.Bounds.center.y + levelElement.Bounds.extents.y) ) {
                        return;
                    }
                }

                _floorTrigger = collider;
                _allowSecondJump = true;

                // if ( _wallTrigger ) {
                //     _wallTrigger = null;
                // }
            }
        }

        void OnCollisionEnter2D(Collision2D other) {
            for ( var i = 0; i < other.contactCount; i++ ) {
                var contactPoint = other.GetContact(i).point;
                if ( Bounds.Contains(contactPoint) ) {
                    return;
                }
        
                var normal = other.GetContact(i).normal;
                CheckWall(other.collider, normal);
                CheckFloor(other.collider, normal);
            }
        }

        void OnCollisionExit2D(Collision2D other) {
            if ( other.collider == _wallTrigger ) {
                _wallTrigger = null;
            }

            if ( other.collider == _floorTrigger ) {
                _floorTrigger = null;
            }

            _jumpProcess = false;
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(Bounds.center, 0.05f);
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }
    }
}