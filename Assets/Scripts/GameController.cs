using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject _player1;
    [SerializeField]
    private GameObject _player2;

    private MovementController _movementControllerPlayer1;
    private MovementController _movementControllerPlayer2;

    private Rigidbody _rigidBodyPlayer1;
    private Rigidbody _rigidBodyPlayer2;

    private Vector3 _spawnPointPlayer1;
    private Vector3 _spawnPointPlayer2;

    private Quaternion _spawnRotationPlayer1;
    private Quaternion _spawnRotationPlayer2;

    private float _waitTime = 0.5f;

    private void Start()
    {
        _movementControllerPlayer1 = _player1.GetComponent<MovementController>();
        _movementControllerPlayer2 = _player2.GetComponent<MovementController>();

        _rigidBodyPlayer1 = _player1.GetComponent<Rigidbody>();
        _rigidBodyPlayer2 = _player2.GetComponent<Rigidbody>();

        _spawnPointPlayer1 = _player1.transform.position;
        _spawnPointPlayer2 = _player2.transform.position;

        _spawnRotationPlayer1 = _player1.transform.rotation;
        _spawnRotationPlayer2 = _player2.transform.rotation;
}

    /// <summary>
    /// Destroys the players after being hit by an enemy.
    /// </summary>
    public void DestroyPlayers()
    {
        _player1.SetActive(false);
        _player2.SetActive(false);

        StartCoroutine(Wait());
    }

    /// <summary>
    /// Wait a bit before respawning the players.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_waitTime);
        RespawnPlayers();
    }

    /// <summary>
    /// Respawns the players after being desroyed by an enemy.
    /// </summary>
    private void RespawnPlayers()
    {
        RestorePlayers();

        _player1.SetActive(true);
        _player2.SetActive(true);
    }

    /// <summary>
    /// Restores the players to the correct orientation.
    /// </summary>
    private void RestorePlayers()
    {
        _movementControllerPlayer1.IsGravityReversed = false;
        _movementControllerPlayer2.IsGravityReversed = false;
        _movementControllerPlayer1.EnteredGravityPortal = false;
        _movementControllerPlayer2.EnteredGravityPortal = false;

        _player1.transform.position = _spawnPointPlayer1;
        _player2.transform.position = _spawnPointPlayer2;

        _player1.transform.rotation = _spawnRotationPlayer1;
        _player2.transform.rotation = _spawnRotationPlayer2;
    }
}
