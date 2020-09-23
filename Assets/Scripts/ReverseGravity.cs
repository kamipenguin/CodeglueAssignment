using UnityEngine;

public class ReverseGravity : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MovementController playerMovementController = other.GetComponent<MovementController>();
            Rigidbody playerRigidBody = other.GetComponent<Rigidbody>();
            if (!playerMovementController.IsGravityReversed)
            {
                playerMovementController.IsGravityReversed = true;
                playerRigidBody.useGravity = false;
                // flip character
            }
            else
            {
                playerMovementController.IsGravityReversed = false;
                playerRigidBody.useGravity = true;
                // flip character
            }
        }
    }
}
