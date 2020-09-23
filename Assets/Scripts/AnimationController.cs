using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Animator _animator;

    private bool _playedJumpAnimation;

    private void Awake()
    {
        _rigidBody = GetComponentInParent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Set the animation to the idle animation when the horizontal velocity is zero.
    /// </summary>
    public void SetIdleAnimation()
    {
        //_animator.SetBool("IsJumping", false);
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsIdling", true);
    }


    /// <summary>
    /// Set the animation to the walking animation when the horizontal velocity is not zero.
    /// </summary>
    public void SetWalkingAnimation()
    {
        //_animator.SetBool("IsJumping", false);
        _animator.SetBool("IsIdling", false);
        _animator.SetBool("IsWalking", true);

        if (_rigidBody.velocity.x > 0)
        {
            //turn model
        }
        else if (_rigidBody.velocity.x < 0)
        {
            //turn model
        }
    }

    /// <summary>
    /// Sets the animation to the jumping animation.
    /// </summary>
    public void SetJumpingAnimation()
    {
        _animator.SetBool("IsIdling", false);
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsJumping", true);
    }
}
