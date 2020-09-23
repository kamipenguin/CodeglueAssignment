using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreedSpirit : MonoBehaviour
{
    private SphereCollider _sphereCollider;
    private float _waitTime = 0.15f;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        Wait();
    }

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
