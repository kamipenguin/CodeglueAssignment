using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Animator _animator;

    private void Awake()
    {
        _rigidBody = GetComponentInParent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Set the animation to the idle animation.
    /// </summary>
    public void SetIdleAnimation()
    {
        _animator.SetBool("IsJumping", false);
        _animator.SetBool("IsWalking", false);
        _animator.SetBool("IsIdling", true);
    }


    /// <summary>
    /// Set the animation to the walking animation.
    /// </summary>
    public void SetWalkingAnimation()
    {
        _animator.SetBool("IsJumping", false);
        _animator.SetBool("IsIdling", false);
        _animator.SetBool("IsWalking", true);
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
