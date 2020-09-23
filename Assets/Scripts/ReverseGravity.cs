using UnityEngine;
using DG.Tweening;

public class ReverseGravity : MonoBehaviour
{
    private float _turnRotation = 180f;
    [SerializeField]
    private float _turnSpeed = 0.1f;

    private void OnTriggerExit(Collider other)
    {
        // turn the player upside down when they went through the gravity portal.
        if (other.gameObject.CompareTag("Player"))
        {
            MovementController playerMovementController = other.GetComponent<MovementController>();
            Rigidbody playerRigidBody = other.GetComponent<Rigidbody>();

            Vector3 playerRotation = playerMovementController.gameObject.transform.eulerAngles;
            playerRigidBody.DORotate(new Vector3(playerRotation.x + _turnRotation, playerRotation.y, playerRotation.z), _turnSpeed);

            playerMovementController.IsGravityReversed = !playerMovementController.IsGravityReversed;

            // store the falling speed the first time the player go through the portal so we can restrict the falling speed.
            if (!playerMovementController.EnteredGravityPortal)
                playerMovementController.StoredMaxFallingSpeed = playerRigidBody.velocity.y;
            else
                playerMovementController.StoredMaxFallingSpeed = -playerMovementController.StoredMaxFallingSpeed;

            playerMovementController.EnteredGravityPortal = true;
        }
    }
}
