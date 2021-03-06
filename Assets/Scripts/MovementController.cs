﻿using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Walk Settings")]
    [SerializeField]
    private float _moveAcceleration = 1f;
    [SerializeField]
    private float _maxMoveSpeed = 10f;
    [SerializeField]
    private float _moveDeceleration = 5f;

    [Header("Jump Settings")]
    [SerializeField]
    private float _initialJumpForce = 5f;
    [SerializeField]
    private float _jumpDeceleration = 0.5f;

    [Header("Gravity Settings")]
    [SerializeField]
    private float _gravityForce = 9.81f;
    [SerializeField]
    private float _minJumpSpeed = 2f;

    private Rigidbody _rigidBody;
    private AnimationController _animationController;

    // harcoded values to achieve desired rotation as mirror on the y-axis doesn't work.
    // I want that the players always face the camera.
    private float _rightRotationUp = 90f;
    private float _leftRotationUp = 130f;
    private float _rightRotationDown = 130f;
    private float _leftRotationDown = 30f;

    private float _currentSpeed;
    private float _currentJumpSpeed;

    private bool _isJumping;
    private bool _stoppedJumping;

    public bool IsGrounded { get; private set; }
    public bool IsGravityReversed { get; set; }
    public bool EnteredGravityPortal { get; set; }
    public float StoredMaxFallingSpeed { get; set; }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animationController = GetComponentInChildren<AnimationController>();
    }

    private void FixedUpdate()
    {
        HandleGravity();
    }

    /// <summary>
    /// Handles the gravity.
    /// </summary>
    private void HandleGravity()
    {
        // check in which direction gravity should be applied.
        if (IsGravityReversed)
            _rigidBody.AddForce(Vector3.up * _gravityForce);
        else
            _rigidBody.AddForce(Vector3.down * _gravityForce);

        // restrict falling velocity when going through the gravity portal.
        if (EnteredGravityPortal)
            LimitFallingSpeed();
    }

    /// <summary>
    /// Limit falling speed between boundaries.
    /// </summary>
    private void LimitFallingSpeed()
    {
        float yVelocity = _rigidBody.velocity.y;

        float limitedFallingSpeed;
        if (IsGravityReversed)
            limitedFallingSpeed = Mathf.Clamp(yVelocity, StoredMaxFallingSpeed, -StoredMaxFallingSpeed);
        else
            limitedFallingSpeed = Mathf.Clamp(yVelocity, -StoredMaxFallingSpeed, StoredMaxFallingSpeed);

        _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, limitedFallingSpeed);
    }

    /// <summary>
    /// Handles the walking movement of the player.
    /// </summary>
    public void Move(float horizontal)
    {
        // when player changes direction, set speed to 0.
        if ((_rigidBody.velocity.x > 0 && horizontal < 0) || (_rigidBody.velocity.x < 0 && horizontal > 0))
            _currentSpeed = 0;

        // accelerate the player's velocity to max speed.
        _currentSpeed += _moveAcceleration * Time.deltaTime;
        _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxMoveSpeed);
        _rigidBody.velocity = new Vector3(horizontal * _currentSpeed, _rigidBody.velocity.y);

        if (IsGrounded)
        {
            _animationController.SetWalkingAnimation();
            TurnPlayer();
        }
    }

    /// <summary>
    /// Turns the player model to the correct direction when walking.
    /// </summary>
    private void TurnPlayer()
    {
        if (EnteredGravityPortal)
            return;

        Vector3 rotation = _rigidBody.transform.eulerAngles;

        // player is walking right.
        if (_rigidBody.velocity.x > 0)
        {
            if (IsGravityReversed)
                rotation.y = _rightRotationDown;
            else
                rotation.y = _rightRotationUp;
        }
        // player is walking left
        else if (_rigidBody.velocity.x < 0)
        {
            if (IsGravityReversed)
                rotation.y = _leftRotationDown;
            else
                rotation.y = _leftRotationUp;
        }

        _rigidBody.transform.eulerAngles = rotation;
    }

    /// <summary>
    /// Handles the stopping movement of the player.
    /// </summary>
    public void StopMoving(float lastHorizontal)
    {
        // decelerate the player's velocity to 0, so the player stops moving.
        if (_currentSpeed != 0)
        {
            _currentSpeed -= _moveDeceleration * Time.deltaTime;
            _currentSpeed = Mathf.Clamp(_currentSpeed, 0, _maxMoveSpeed);
            _rigidBody.velocity = new Vector3(lastHorizontal * _currentSpeed, _rigidBody.velocity.y);
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
            _isJumping = true;
            _stoppedJumping = false;

            if (IsGravityReversed)
                _currentJumpSpeed = -_initialJumpForce;
            else
                _currentJumpSpeed = _initialJumpForce;

            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _currentJumpSpeed);

            _animationController.SetJumpingAnimation();
        }
        // if the player is still jumping, decelerate the upwards velocity to slow the jump.
        else if (_isJumping)
        {
            float deceleration = _jumpDeceleration * Time.deltaTime;
            if (IsGravityReversed)
                _currentJumpSpeed += deceleration;
            else
                _currentJumpSpeed -= deceleration;

            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _currentJumpSpeed);
        }
    }

    /// <summary>
    /// Handles the falling down of the player.
    /// </summary>
    public void HandleFall()
    {
        if (EnteredGravityPortal)
            return;

        // check if the player is still in the air.
        if (!IsGrounded)
        {
            // if the player just started stopping the jump, set the jump velocity to a small value so the player's upwards velocity decelerates fast.
            if (!_stoppedJumping)
            {
                _stoppedJumping = true;

                float maxVelocity;
                if (IsGravityReversed)
                    maxVelocity = Mathf.Clamp(_rigidBody.velocity.y, -_minJumpSpeed, 0);
                else
                    maxVelocity = Mathf.Clamp(_rigidBody.velocity.y, 0, _minJumpSpeed);

                _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, maxVelocity);
            }
        }

        _isJumping = false;
    }

    /// <summary>
    /// Checks if the player is on the ground or on a player.
    /// </summary>
    /// <returns></returns>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            IsGrounded = true;
            EnteredGravityPortal = false;
        }
    }
}
