using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{   
    [SerializeField]
    private GameObject _crawler;
    [SerializeField]
    private GameObject _freedSpiritPrefab;

    private GameObject _parentObject;

    private float _shootSpiritForce = 7.5f;

    private void Awake()
    {
        _parentObject = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject freedSpirit = Instantiate(_freedSpiritPrefab, transform.position, Quaternion.identity);
            Rigidbody freedSpiritRigidbody = freedSpirit.GetComponent<Rigidbody>();
            freedSpiritRigidbody.AddForce(Vector3.up * _shootSpiritForce, ForceMode.Impulse);

            Destroy(_parentObject);
        }
    }
}
