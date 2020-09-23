using UnityEngine;

public class InputController : MonoBehaviour
{
    private MovementController _movementController;
    private enum Player { Player1, Player2 };
    [SerializeField]
    private Player _player;

    [SerializeField]
    private float _maxJumpButtonHoldTime = 0.25f;
    private float _jumpButtonHoldTimer = 0f;

    private float _storeHorizontal;

    private string _player1Horizontal = "Horizontal";
    private string _player1Jump = "Jump";

    private string _player2Horizontal = "Horizontal2";
    private string _player2Jump = "Jump2";

    private void Awake()
    {
        _movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        if (_player == Player.Player1)
            HandleInput(_player1Horizontal, _player1Jump);
        else
            HandleInput(_player2Horizontal, _player2Jump);
    }

    /// <summary>
    /// Handles the input.
    /// </summary>
    private void HandleInput(string horizontalName, string jumpName)
    {
        // handles input for walking.
        float horizontal = Input.GetAxisRaw(horizontalName);
        if (horizontal != 0)
        {
            _storeHorizontal = horizontal;
            _movementController.Move(horizontal);
        }
        else
            _movementController.StopMoving(_storeHorizontal);

        // handles input for jumping.
        if (Input.GetAxisRaw(jumpName) > 0)
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
