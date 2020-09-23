using UnityEngine;

public class Spirit : MonoBehaviour
{
    [SerializeField]
    private GameObject _crawler;
    [SerializeField]
    private GameObject _freedSpiritPrefab;
    [SerializeField]
    private float _shootSpiritForce = 7.5f;

    private GameObject _parentObject;


    private void Awake()
    {
        _parentObject = transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        // spawn the freed spirit and shoot it upwards.
        GameObject freedSpirit = Instantiate(_freedSpiritPrefab, transform.position, Quaternion.identity);
        Rigidbody freedSpiritRigidbody = freedSpirit.GetComponent<Rigidbody>();
        freedSpiritRigidbody.AddForce(Vector3.up * _shootSpiritForce, ForceMode.Impulse);

        Destroy(_parentObject);
    }
}
