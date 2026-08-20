using UnityEngine;
using UnityEngine.InputSystem;

public class sLocalPlayerManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    private int playerCount = 0;

    public void OnPlayerJoined(PlayerInput player)
    {
        Debug.Log($"Player {playerCount + 1} joined!");

        if (playerCount < spawnPoints.Length)
        {
            player.transform.position = spawnPoints[playerCount].position;
            player.transform.rotation = spawnPoints[playerCount].rotation;
        }

        playerCount++;
    }
}