using UnityEngine;
using EventsHelper;

using KeyAnim = AnimatorControl.KeyAnim;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BaseAnimationControl))]
public class PlayerControl : MonoBehaviour {
    public float   Speed         = 10;
    public Vector2 JumpForce     = Vector2.one;
    public Vector2 SlideVelosity = Vector2.right;
    public Bounds  LocalBounds   = new Bounds();

    Rigidbody2D          _rb               = null;
    BaseAnimationControl _animationControl = null;
    Collider2D           _wallTrigger      = null;
    Collider2D           _floorTrigger     = null;
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
            var worldBounds = LocalBounds;
            worldBounds.center = transform.TransformPoint(LocalBounds.center);
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
        } else if ( CanSlideInWall ) {
            SlideInWall();
        }
    }

    void Jump(bool secondJump = false) {
        var jumpForce = JumpForce;
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
        var slideVelosity = SlideVelosity;
        slideVelosity.x = (transform.localScale.x > 0) ? -Mathf.Abs(slideVelosity.x) : Mathf.Abs(slideVelosity.x);
        _rb.velocity = slideVelosity;
        _animationControl.PlayAnimation(KeyAnim.SlideInWall);
    }

    void MoveLeftOrRight() {
        ChangeMirrorScale();
        var dir = (transform.localScale.x > 0) ? Vector2.left : Vector2.right;
        var movePos = (Vector2)transform.position + (dir * Speed * Time.fixedDeltaTime);
        _rb.MovePosition(movePos);
        _animationControl.PlayAnimation(KeyAnim.Walk);
    }

    void ChangeMirrorScale() {
        var curScale = transform.localScale;
        curScale.x = (_wallNormal == Vector2.right) ? -Mathf.Abs(curScale.x) : Mathf.Abs(curScale.x);
        transform.localScale = curScale;
    }

    bool IsWall(Vector2 normal) {
        return (normal == Vector2.left) || (normal == Vector2.right);
    }

    bool IsFloor(Vector2 normal) {
        return (normal == Vector2.up);
    }

    void OnCollisionEnter2D(Collision2D other) {
        var destructionObject = other.gameObject.GetComponent<DestructionObject>();
        if ( destructionObject ) {
            EventManager.Fire<DestructionObjectPlayerCollision>(new DestructionObjectPlayerCollision(destructionObject));
            return;
        }

        var contactPoint = other.GetContact(0).point;
        if ( Bounds.Contains(contactPoint) ) {
            return;
        }
 
        var normal = other.GetContact(0).normal;
        if ( IsWall(normal) && !_wallTrigger ) {
            _wallTrigger = other.collider;
            _wallNormal = normal;
            _allowSecondJump = true;
        }

        if ( IsFloor(normal) && !_floorTrigger ) {
            var point = other.GetContact(0).point;
            _floorTrigger = other.collider;
            _allowSecondJump = true;
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