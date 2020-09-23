using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private string _playerHorizontal;
    [SerializeField]
    private string _playerJump;

    [SerializeField]
    private float _maxJumpButtonHoldTime = 0.25f;

    private float _jumpButtonHoldTimer = 0f;

    private float _storeHorizontal;

    private MovementController _movementController;

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        HandleWalkInput();
        HandleJumpInput();
    }

    /// <summary>
    /// Handles the input for walking.
    /// </summary>
    private void HandleWalkInput()
    {
        float horizontal = Input.GetAxisRaw(_playerHorizontal);
        if (horizontal != 0)
        {
            _storeHorizontal = horizontal;
            _movementController.Move(horizontal);
        }
        else
            _movementController.StopMoving(_storeHorizontal);
    }

    /// <summary>
    /// Handles the input for jumping.
    /// </summary>
    private void HandleJumpInput()
    {
        if (Input.GetAxisRaw(_playerJump) > 0)
        {
            _jumpButtonHoldTimer += Time.deltaTime;
            // check how long the jump button has been pressed.
            if (_jumpButtonHoldTimer < _maxJumpButtonHoldTime)
                _movementController.Jump();
            else
                _movementController.HandleFall();
        }
        else
            _movementController.HandleFall();

        // resets jump timer when the player is grounded.
        if (_movementController.IsGrounded)
            _jumpButtonHoldTimer = 0;
    }
}
