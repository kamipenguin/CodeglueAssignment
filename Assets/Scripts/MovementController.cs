using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private AnimationController _animationController;

    [Header("Walk Settings")]
    [SerializeField]
    private float _moveAcceleration = 1f;
    [SerializeField]
    private float _maxMoveSpeed = 10f;
    [SerializeField]
    private float _moveDeceleration = 5f;

    [Header("Jump Settings")]
    [SerializeField]
    private float _initialJumpForce = 100f;
    [SerializeField]
    private float _jumpDeceleration = 10f;
    [SerializeField]
    private float _minJumpVelocity = 20f;

    private float _currentJumpSpeed;
    private bool _stoppedJumping;

    [Header("Gravity Settings")]
    [SerializeField]
    private float _gravityForce = 10f;

    private float _rightRotationUp = 90f;
    private float _leftRotationUp = 130f;

    private float _rightRotationDown = 130f;
    private float _leftRotationDown = 30f;

    public float CurrentSpeed { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsJumping { get; set; }
    public bool IsGravityReversed { get; set; }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animationController = GetComponentInChildren<AnimationController>();
    }

    private void FixedUpdate()
    {
        if (IsGravityReversed)
        {
            _rigidBody.AddForce(Vector3.up * _gravityForce);
        }
        else
            _rigidBody.AddForce(Vector3.down * _gravityForce);
    }

    /// <summary>
    /// Handles the walking movement of the player.
    /// </summary>
    /// <param name="horizontal"></param>
    public void Move(float horizontal)
    {
        // when player changes direction, set speed to 0.
        if ((_rigidBody.velocity.x > 0 && horizontal < 0) || (_rigidBody.velocity.x < 0 && horizontal > 0))
            CurrentSpeed = 0;
        // accelerate the player's velocity to max speed.
        CurrentSpeed += _moveAcceleration * Time.deltaTime;
        CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, _maxMoveSpeed);
        _rigidBody.velocity = new Vector2(horizontal * CurrentSpeed, _rigidBody.velocity.y);

        if (IsGrounded)
        {
            _animationController.SetWalkingAnimation();
            TurnPlayer();
        }
    }

    private void TurnPlayer()
    {

        // turn the player in the correct direction
        if (_rigidBody.velocity.x > 0)
        {
            Vector3 currentRotation = _rigidBody.transform.eulerAngles;
            if (IsGravityReversed)
                _rigidBody.transform.eulerAngles = new Vector3(currentRotation.x, _rightRotationDown, currentRotation.z);
            else
                _rigidBody.transform.eulerAngles = new Vector3(currentRotation.x, _rightRotationUp, currentRotation.z);
        }
        else if (_rigidBody.velocity.x < 0)
        {
            Vector3 currentRotation = _rigidBody.transform.eulerAngles;
            if (IsGravityReversed)
                _rigidBody.transform.eulerAngles = new Vector3(currentRotation.x, _leftRotationDown, currentRotation.z);
            else
                _rigidBody.transform.eulerAngles = new Vector3(currentRotation.x, _leftRotationUp, currentRotation.z);
        }
    }

    /// <summary>
    /// Handles the stopping movement of the player.
    /// </summary>
    /// <param name="lastHorizontal"></param>
    public void StopMoving(float lastHorizontal)
    {
        // decelerate the player's velocity to 0, so the player stops moving.
        if (CurrentSpeed != 0)
        {
            CurrentSpeed -= _moveDeceleration * Time.deltaTime;
            CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, _maxMoveSpeed);
            _rigidBody.velocity = new Vector2(lastHorizontal * CurrentSpeed, _rigidBody.velocity.y);
        }
        else if (IsGrounded)
            _animationController.SetIdleAnimation();
    }


    /// <summary>
    /// Handles the jumping of the player.
    /// </summary>
    public void Jump()
    {
        // if the player is on the ground, set the upwards velocity high so the player launches in the air.
        if (IsGrounded)
        {
            IsGrounded = false;
            IsJumping = true;
            _stoppedJumping = false;
            _currentJumpSpeed = _initialJumpForce;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _currentJumpSpeed);

            _animationController.SetJumpingAnimation();
        }
        // if the player is still jumping, decelerate the upwards velocity to slow the jump.
        else if (IsJumping)
        {
            _currentJumpSpeed -= _jumpDeceleration * Time.deltaTime;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _currentJumpSpeed);
        }
    }

    /// <summary>
    /// Handles the stopping of the jump of the player.
    /// </summary>
    public void StopJumping()
    {
        // check if the player is still in the air.
        if (!IsGrounded)
        {
            // the first time the player's jump is stopped, set the jump velocity to a small value so the player's upwards velocity decelerates fast.
            if (!_stoppedJumping)
            {
                _stoppedJumping = true;
                float minVelocity = Mathf.Clamp(_rigidBody.velocity.y, 0, _minJumpVelocity);
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, minVelocity);
            }
        }

        IsJumping = false;
    }

    /// <summary>
    /// Checks if the player is on the ground or on a player.
    /// </summary>
    /// <returns></returns>
    private void OnCollisionEnter(Collision collision)
    {
        IsGrounded = collision.gameObject.CompareTag("Ground");
    }
}
