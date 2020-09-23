using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointAgent : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _wayPoints = new List<Transform>();
    [SerializeField]
    private float _moveSpeed = 5;
    [SerializeField]
    private float _waitTime = 0.5f;
    [SerializeField]
    private float _goalReachedDistance = 0.2f;

    private int _currentIndex = 0;
    private bool _hasReachedDestination;

    private void Update()
    {
        MoveToDestination();
    }

    /// <summary>
    /// Moves the enemy to destination if they hasn't reached it yet.
    /// </summary>
    private void MoveToDestination()
    {
        if (_hasReachedDestination)
            return;

        Vector3 currentPosition = transform.position;
        Vector3 goalPosition = _wayPoints[_currentIndex].position;
        gameObject.transform.position = Vector3.MoveTowards(currentPosition, goalPosition, _moveSpeed);

        CheckDestinationReached(currentPosition, goalPosition);
    }

    /// <summary>
    /// Checks if the enemy has reached their destination.
    /// </summary>
    /// <param name="currentPos"></param>
    /// <param name="goalPos"></param>
    private void CheckDestinationReached(Vector3 currentPos, Vector3 goalPos)
    {
        if (Vector3.Distance(currentPos, goalPos) <= _goalReachedDistance)
        {
            _hasReachedDestination = true;
            StartCoroutine(WaitAndTurn());
        }
    }

    /// <summary>
    /// Wait for a few seconds before turning around and sets new destination.
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAndTurn()
    {
        yield return new WaitForSeconds(_waitTime);

        if (_currentIndex == 0)
            _currentIndex = 1;
        else
            _currentIndex = 0;

        _hasReachedDestination = false;
    }
}
