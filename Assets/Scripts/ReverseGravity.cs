using UnityEngine;
using DG.Tweening;

public class ReverseGravity : MonoBehaviour
{
    [SerializeField]
    private float _turnSpeed = 0.1f;

    private float _turnRotation = 180f;

    private void OnTriggerExit(Collider other)
    {
        // turn the player upside down when they went through the gravity portal.
        if (!other.gameObject.CompareTag("Player"))
            return;

        MovementController playerMovementController = other.GetComponent<MovementController>();
        Rigidbody playerRigidBody = other.GetComponent<Rigidbody>();

        Vector3 playerRotation = playerMovementController.transform.eulerAngles;
        Vector3 targetRotation = new Vector3(playerRotation.x + _turnRotation, playerRotation.y, playerRotation.z);
        playerRigidBody.DORotate(targetRotation, _turnSpeed);

        playerMovementController.IsGravityReversed = !playerMovementController.IsGravityReversed;

        // store the falling speed the first time the player go through the portal so we can restrict the falling speed.
        if (!playerMovementController.EnteredGravityPortal)
            playerMovementController.StoredMaxFallingSpeed = playerRigidBody.velocity.y;
        else
            playerMovementController.StoredMaxFallingSpeed = -playerMovementController.StoredMaxFallingSpeed;

        playerMovementController.EnteredGravityPortal = true;
    }
}
