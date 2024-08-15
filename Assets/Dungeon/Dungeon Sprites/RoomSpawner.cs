using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    private GameObject grid;
    public int openingDirection;
    // 1 Down door     2 Top door        3  Left door       4 Right door

    private RoomTemplates templates;
    public bool spawned = false;
    public bool spawnedClosedRoom=false;
    public bool bossRoom = false;
    void Start()
    {
        grid = GameObject.Find("Grid");
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
        Invoke("SpawnRoomConectors", 3);
    }

    void Spawn()
    {
        if (!spawned)
        {
            // Verificar si el espacio está vacío antes de instanciar
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.0001f);
            if (colliders.Length <= 1 && templates.roomsGenerated < templates.roomMin)
            {
                if (openingDirection == 1)
                {
                    int rand = Random.Range(0, templates.NTdownRooms.Length);
                    Instantiate(templates.NTdownRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 2)
                {
                    int rand = Random.Range(0, templates.NTtopRooms.Length);
                    Instantiate(templates.NTtopRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 3)
                {
                    int rand = Random.Range(0, templates.NTleftRooms.Length);
                    Instantiate(templates.NTleftRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                else if (openingDirection == 4)
                {
                    int rand = Random.Range(0, templates.NTrightRooms.Length);
                    Instantiate(templates.NTrightRooms[rand], transform.position, Quaternion.identity, grid.transform);
                }
                spawned = true;
                templates.roomsGenerated++;
            }
            else if (colliders.Length <= 1 && templates.roomsGenerated < templates.roomsLimit)
            {
                if (openingDirection == 1)
                {
                    int rand = Random.Range(0, templates.downRooms.Length);
                    Instantiate(templates.downRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnBoosRoom(rand,templates.downRooms,"D",false);
                }
                else if (openingDirection == 2)
                {
                    int rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnBoosRoom(rand, templates.topRooms, "T",false);
                }
                else if (openingDirection == 3)
                {
                    int rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnBoosRoom(rand, templates.leftRooms, "L",false);
                }
                else if (openingDirection == 4)
                {
                    int rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], transform.position, Quaternion.identity, grid.transform);
                    SpawnBoosRoom(rand, templates.rightRooms, "R", false);
                }
                spawned = true;
                templates.roomsGenerated++;
            }
            else if(colliders.Length <= 1 && templates.roomsGenerated >= templates.roomsLimit)
            {
                if (openingDirection == 1)
                {
                    List<GameObject> downList = new List<GameObject>(templates.downRooms);
                    GameObject downRoom = downList.Find(go => go.name == "D");
                    Instantiate(downRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnBoosRoom(1607, templates.downRooms, "D", true);

                }
                else if (openingDirection == 2)
                {
                    List<GameObject> upList = new List<GameObject>(templates.topRooms);
                    GameObject upRoom = upList.Find(go => go.name == "T");
                    Instantiate(upRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnBoosRoom(1607, templates.downRooms, "D", true);
                }
                else if (openingDirection == 3)
                {
                    List<GameObject> leftList = new List<GameObject>(templates.leftRooms);
                    GameObject leftRoom = leftList.Find(go => go.name == "L");
                    Instantiate(leftRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnBoosRoom(1607, templates.downRooms, "D", true);
                }
                else if (openingDirection == 4)
                {
                    List<GameObject> rightList = new List<GameObject>(templates.rightRooms);
                    GameObject rightRoom = rightList.Find(go => go.name == "R");
                    Instantiate(rightRoom, transform.position, Quaternion.identity, grid.transform);
                    SpawnBoosRoom(1607, templates.downRooms, "D", true);
                }
                spawned = true;
                templates.roomsGenerated++;
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("SpawnPoint"))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
            if (col.GetComponent<RoomSpawner>().spawned==false && !spawned && colliders.Length<=2)
            {
                Instantiate(templates.closedRoom,transform.position,Quaternion.identity, grid.transform);
                spawnedClosedRoom = true;
                col.GetComponent<RoomSpawner>().spawnedClosedRoom = true;
                col.GetComponent<RoomSpawner>().spawned = true;
            }
            else if (spawnedClosedRoom && !col.GetComponent<RoomSpawner>().spawned)
            {
                col.GetComponent<RoomSpawner>().spawnedClosedRoom = true;
                col.GetComponent<RoomSpawner>().spawned = true;
            }
            
        }
    }

    // NO siempre crea una BossRoom
    public void SpawnBoosRoom(int rand,GameObject[] rooms, string room, bool closing)
    {
        if (!templates.bossRoomSpawned && rooms[rand].name == room && !bossRoom)
        {
            templates.bossRoomSpawned = true;
            bossRoom = true;
            Debug.Log("SOY LA BOSS ROOM");
            GameObject.Find("FedeTest").transform.position = transform.position;
        }
        else if (closing)
        {
            templates.bossRoomSpawned = true;
            bossRoom = true;
            Debug.Log("SOY LA BOSS ROOM");
            GameObject.Find("FedeTest").transform.position = transform.position;
        }
    }
    private void SpawnRoomConectors()
    {
        if (!spawnedClosedRoom) Instantiate(templates.roomConector, transform.position, Quaternion.identity);
    }
}
