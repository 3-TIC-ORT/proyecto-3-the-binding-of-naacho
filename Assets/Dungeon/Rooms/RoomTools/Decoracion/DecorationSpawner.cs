using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(RoomSpawner))]
public class DecorationSpawner : MonoBehaviour
{
    private RoomTemplates templates;
    private RoomSpawner roomSpawner;
    private bool spawnObstacles;
    private bool decorationSpawned;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        roomSpawner = GetComponent<RoomSpawner>();
        spawnObstacles = Random.value>0.5f;
    }
    void Update()
    {
        if (roomSpawner.spawned && !roomSpawner.spawnedClosedRoom)
        {
            if (spawnObstacles && !roomSpawner.specialRoom)
            {
                SpawnObstacles();
            }
            else if (!spawnObstacles)
            {
                // Spawnear algo normal, como piedritas. Cosas que no afecten al jugador
            }
        }
    }
    // Por ahora solo son tiles.
    private void SpawnObstacles()
    {
        
    }
}
