using UnityEngine;

public class ReverseGravity : MonoBehaviour
{
    private float _turnRotation = 180f;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MovementController playerMovementController = other.GetComponent<MovementController>();
            Rigidbody playerRigidBody = other.GetComponent<Rigidbody>();
            if (!playerMovementController.IsGravityReversed)
            {
                playerMovementController.IsGravityReversed = true;
                Vector3 playerRotation = playerMovementController.gameObject.transform.eulerAngles;
                playerMovementController.gameObject.transform.eulerAngles = new Vector3(_turnRotation, playerRotation.y, playerRotation.z);
            }
            else
            {
                playerMovementController.IsGravityReversed = false;
                Vector3 playerRotation = playerMovementController.gameObject.transform.eulerAngles;
                playerMovementController.gameObject.transform.eulerAngles = new Vector3(_turnRotation, playerRotation.y, playerRotation.z);
            }
        }
    }
}
