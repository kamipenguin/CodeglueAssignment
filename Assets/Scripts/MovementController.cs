﻿using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Animator _animator;

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
    [SerializeField]
    private float _jumpFallDecelation = 2f;

    private float _currentJumpSpeed;
    private bool _stoppedJumping;


    public float CurrentSpeed { get; set; }
    public bool IsGrounded { get; set; }
    public bool IsJumping { get; set; }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
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

        //SetWalkingAnimation();
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

        //SetIdleAnimation();
    }

    /// <summary>
    /// Set the animation to the idle animation when the horizontal velocity is zero.
    /// </summary>
    private void SetIdleAnimation()
    {
        if (_rigidBody.velocity.x == 0)
        {
            _animator.SetBool("IsWalking", false);
            _animator.SetBool("IsIdling", true);
        }
    }


    /// <summary>
    /// Set the animation to the walking animation when the horizontal velocity is not zero.
    /// </summary>
    private void SetWalkingAnimation()
    {
        if (IsGrounded)
        {
            if (_rigidBody.velocity.x > 0)
            {
                _animator.SetBool("IsJumping", false);
                _animator.SetBool("IsIdling", false);
                _animator.SetBool("IsWalking", true);
                //_spriteRenderer.flipX = false;
            }
            else
            {
                _animator.SetBool("IsJumping", false);
                _animator.SetBool("IsIdling", false);
                _animator.SetBool("IsWalking", true);
                //_spriteRenderer.flipX = true;
            }
        }
    }

    /// <summary>
    /// Handles the jumping of the player.
    /// </summary>
    public void Jump()
    {
        CheckIsGrounded();

        // if the player is on the ground, set the upwards velocity high so the player launches in the air.
        if (IsGrounded)
        {
            IsGrounded = false;
            IsJumping = true;
            _stoppedJumping = false;
            _currentJumpSpeed = _initialJumpForce;
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _currentJumpSpeed);

            //SetJumpingAnimation();
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
        CheckIsGrounded();

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
            else
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.y - _jumpFallDecelation);
            }
        }

        IsJumping = false;
    }

    /// <summary>
    /// Sets the animation to the jumping animation.
    /// </summary>
    private void SetJumpingAnimation()
    {
        _animator.SetBool("IsIdling", false);
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsJumping", true);
    }

    /// <summary>
    /// Checks if the player is on the ground or on a player.
    /// </summary>
    /// <returns></returns>
    private void CheckIsGrounded()
    {
        if (_rigidBody.velocity.y == 0)
        {
                //_animator.SetBool("IsWalking", false);
                //_animator.SetBool("IsJumping", false);
                //_animator.SetBool("IsIdling", true);
                IsGrounded = true;
        }
        else
            IsGrounded = false;
    }
}
