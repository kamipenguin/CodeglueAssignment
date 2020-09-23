using System.Collections;
using UnityEngine;

public class FreedSpirit : MonoBehaviour
{
    [SerializeField]
    private float _waitTime = 0.15f;

    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    /// <summary>
    /// This gameobject spawned immediatly after the player defeated an enemy.
    /// Wait a bit before setting the isTrigger to false so it won't be destroyed the moment the player defeated the enemy.
    /// After setting the isTrigger to false, collision can be detected again.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_waitTime);
        _sphereCollider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
}
