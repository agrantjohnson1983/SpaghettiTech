using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class sPlayerManager : MonoBehaviour
{
    public static sPlayerManager Instance;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform[] spawnPoints;

    public List<PlayerInput> players = new List<PlayerInput>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayerJoined(PlayerInput player)
    {
        if (!players.Contains(player))
            players.Add(player);

        int playerIndex = players.Count - 1;

        if (playerIndex < spawnPoints.Length)
        {
            player.transform.position = spawnPoints[playerIndex].position;
            player.transform.rotation = spawnPoints[playerIndex].rotation;
        }

        Debug.Log($"Player {playerIndex + 1} joined.");
    }

    public void PlayerLeft(PlayerInput player)
    {
        players.Remove(player);

        Debug.Log("Player left.");
    }
}