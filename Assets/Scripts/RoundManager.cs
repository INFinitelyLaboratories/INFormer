using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public sealed class RoundManager : MonoBehaviour
{
    [SerializeField] private List<Vector3> m_spawnPositions = new List<Vector3>();
    
    private List<Vector3> m_freeSpawnPositions = new List<Vector3>();

    private List<PlayerMovement> m_players = new List<PlayerMovement>();

    private void Awake()
    {
        m_players = FindObjectsOfType<PlayerMovement>().ToList();

        SpawnPlayers( m_players );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SpawnPlayers( m_players );
    }

    private void SpawnPlayers( List<PlayerMovement> players )
    {
        if (players == null) return;
        if (players.Count == 0) return;

        m_freeSpawnPositions = m_spawnPositions.AsReadOnly().ToList();

        foreach( PlayerMovement player in players )
        {
            Vector3 position = m_freeSpawnPositions[ Random.Range( 0 , m_freeSpawnPositions.Count ) ];

            player.transform.position = position;

            m_freeSpawnPositions.Remove( position );
        }
    }
}
