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
    private GameObject decorationContainer;
    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        decorationContainer = GameObject.FindGameObjectWithTag("DecorationContainer");
        roomSpawner = gameObject.GetComponent<RoomSpawner>();
        spawnObstacles = Random.value>0.5f;
    }
    void Update()
    {
        if(roomSpawner == null) {
            Start();
            return;
        }
        if (roomSpawner.spawned && !roomSpawner.spawnedClosedRoom)
        {
            if (spawnObstacles && !decorationSpawned && !roomSpawner.specialRoom && !roomSpawner.showInMapInStart)
            {
                SpawnObstacles();
            }
            else if (!spawnObstacles && !decorationSpawned)
            {
                // Spawnear algo normal, como piedritas. Cosas que no afecten al jugador
            }
        }
    }
    // Por ahora solo son tiles.
    private void SpawnObstacles()
    {
        decorationSpawned = true;
        int rand = Random.Range(0,templates.obstacles.Length);
        Instantiate(templates.obstacles[rand],transform.position,Quaternion.identity,decorationContainer.transform);
    }
}
