using UnityEngine;

public class InputController : MonoBehaviour
{
    private MovementController _movementController;
    [SerializeField]
    private float _maxJumpButtonHoldTime = 0.5f;
    private float _jumpButtonHoldTimer = 0f;

    private float _storeHorizontal;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        HandleInput();
    }

    /// <summary>
    /// Handles the input.
    /// </summary>
    private void HandleInput()
    {
        // handles input for walking.
        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            _storeHorizontal = horizontal;
            _movementController.Move(horizontal);
        }
        else
            _movementController.StopMoving(_storeHorizontal);

        // handles input for jumping.
        if (Input.GetAxisRaw("Jump") > 0)
        {
            _jumpButtonHoldTimer += Time.deltaTime;
            // check how long the jump button has been pressed.
            if (_jumpButtonHoldTimer < _maxJumpButtonHoldTime)
                _movementController.Jump();
            else
                _movementController.StopJumping();
        }
        else
            _movementController.StopJumping();

        // resets jump timer when the player is grounded.
        if (_movementController.IsGrounded)
            _jumpButtonHoldTimer = 0;
    }
}
